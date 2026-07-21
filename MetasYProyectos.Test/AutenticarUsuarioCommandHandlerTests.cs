using System.Data;
using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Application.Autenticacion.Excepciones;
using MetasYProyectos.Domain.Autenticacion;
using Moq;
using Xunit;

namespace MetasYProyectos.Test;

public class AutenticarUsuarioCommandHandlerTests
{
    private readonly Mock<IOracleConnectionFactory> _factoryMock = new();
    private readonly Mock<IDbConnection> _techConnMock = new();
    private readonly Mock<IDbConnection> _userConnMock = new();

    private AutenticarUsuarioCommandHandler Sut(IList<IPasoValidacionLogin>? pasos = null)
    {
        _factoryMock.Setup(f => f.CrearConexionTecnicaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_techConnMock.Object);
        _factoryMock.Setup(f => f.CrearConexionUsuarioAsync(It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_userConnMock.Object);

        pasos ??= new List<IPasoValidacionLogin>
        {
            PasoMock(1, true, null!).Object,
            PasoMock(2, true, null!).Object,
            PasoMock(4, true, null!).Object,
            PasoMock(5, true, null!).Object,
            PasoMock(6, true, null!).Object
        };

        return new AutenticarUsuarioCommandHandler(_factoryMock.Object, pasos);
    }

    private static Mock<IPasoValidacionLogin> PasoMock(int orden, bool exitoso, string? mensaje)
    {
        var mock = new Mock<IPasoValidacionLogin>();
        mock.Setup(p => p.Orden).Returns(orden);
        mock.Setup(p => p.ValidarAsync(It.IsAny<IDbConnection>(), It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exitoso ? ResultadoValidacion.Ok() : ResultadoValidacion.Fallo(mensaje!));
        return mock;
    }

    private static CredencialesLogin Creds() => new("TESTUSER", "pass123", "VIGENCIA", "MiBase");

    [Fact]
    public async Task Ok_devuelve_resultado_exitoso()
    {
        var handler = Sut();
        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.True(result.Existoso);
        Assert.Null(result.MensajeError);
    }

    [Theory]
    [InlineData(1, "El esquema o vigencia no existe")]
    [InlineData(2, "El usuario no existe")]
    [InlineData(4, "Acceso denegado")]
    [InlineData(5, "El usuario se encuentra inactivo")]
    [InlineData(6, "El usuario no tiene permisos")]
    public async Task Paso_falla_devuelve_mensaje(int ordenFallo, string esperado)
    {
        var pasos = new List<IPasoValidacionLogin>
        {
            PasoMock(1, ordenFallo != 1, ordenFallo == 1 ? esperado : null).Object,
            PasoMock(2, ordenFallo != 2, ordenFallo == 2 ? esperado : null).Object,
            PasoMock(4, ordenFallo != 4, ordenFallo == 4 ? esperado : null).Object,
            PasoMock(5, ordenFallo != 5, ordenFallo == 5 ? esperado : null).Object,
            PasoMock(6, ordenFallo != 6, ordenFallo == 6 ? esperado : null).Object
        };

        var handler = Sut(pasos);
        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.False(result.Existoso);
        Assert.Contains(esperado, result.MensajeError);
    }

    [Fact]
    public async Task Credenciales_invalidas_devuelve_error()
    {
        var handler = Sut();
        _factoryMock.Setup(f => f.CrearConexionUsuarioAsync(It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CredencialesInvalidasException());

        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.False(result.Existoso);
    }

    [Fact]
    public async Task Conexion_db_falla_devuelve_error()
    {
        var handler = Sut();
        _factoryMock.Setup(f => f.CrearConexionTecnicaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ConexionBaseDatosException("DB down", new Exception()));

        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.False(result.Existoso);
        Assert.Contains("DB down", result.MensajeError);
    }

    [Fact]
    public async Task Excepcion_generica_devuelve_mensaje_generico()
    {
        var handler = Sut();
        _factoryMock.Setup(f => f.CrearConexionTecnicaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("unexpected"));

        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.False(result.Existoso);
        Assert.Contains("No fue posible validar", result.MensajeError);
    }

    [Fact]
    public async Task Pasos_se_ejecutan_en_orden()
    {
        var ordenEjecucion = new List<int>();

        var pasos = new List<IPasoValidacionLogin>
        {
            PasoConCallback(5, true, ordenEjecucion).Object,
            PasoConCallback(1, true, ordenEjecucion).Object,
            PasoConCallback(6, true, ordenEjecucion).Object,
            PasoConCallback(2, true, ordenEjecucion).Object,
            PasoConCallback(4, true, ordenEjecucion).Object
        };

        var handler = Sut(pasos);
        await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.Equal(new List<int> { 1, 2, 4, 5, 6 }, ordenEjecucion);
    }

    [Fact]
    public async Task Paso_falla_detiene_ejecucion()
    {
        var ordenEjecucion = new List<int>();

        var pasos = new List<IPasoValidacionLogin>
        {
            PasoConCallback(1, true, ordenEjecucion).Object,
            PasoConCallback(2, false, ordenEjecucion, "fail").Object,
            PasoConCallback(4, true, ordenEjecucion).Object,
            PasoConCallback(5, true, ordenEjecucion).Object,
            PasoConCallback(6, true, ordenEjecucion).Object
        };

        var handler = Sut(pasos);
        var result = await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.False(result.Existoso);
        Assert.Equal(new List<int> { 1, 2 }, ordenEjecucion);
    }

    [Fact]
    public async Task Pasos_mayor_2_usan_conexion_usuario()
    {
        var connUsada = new List<IDbConnection>();

        var pasoTecnico = new Mock<IPasoValidacionLogin>();
        pasoTecnico.Setup(p => p.Orden).Returns(1);
        pasoTecnico.Setup(p => p.ValidarAsync(It.IsAny<IDbConnection>(), It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .Callback<IDbConnection, CredencialesLogin, CancellationToken>((c, _, _) => connUsada.Add(c))
            .ReturnsAsync(ResultadoValidacion.Ok());

        var pasoUsuario = new Mock<IPasoValidacionLogin>();
        pasoUsuario.Setup(p => p.Orden).Returns(4);
        pasoUsuario.Setup(p => p.ValidarAsync(It.IsAny<IDbConnection>(), It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .Callback<IDbConnection, CredencialesLogin, CancellationToken>((c, _, _) => connUsada.Add(c))
            .ReturnsAsync(ResultadoValidacion.Ok());

        var handler = Sut(new List<IPasoValidacionLogin> { pasoTecnico.Object, pasoUsuario.Object });
        await handler.Handle(new AutenticarUsuarioCommand(Creds()), CancellationToken.None);

        Assert.Same(_techConnMock.Object, connUsada[0]);
        Assert.Same(_userConnMock.Object, connUsada[1]);
    }

    private static Mock<IPasoValidacionLogin> PasoConCallback(int orden, bool exitoso, List<int> ordenEjecucion, string? msg = null)
    {
        var mock = new Mock<IPasoValidacionLogin>();
        mock.Setup(p => p.Orden).Returns(orden);
        mock.Setup(p => p.ValidarAsync(It.IsAny<IDbConnection>(), It.IsAny<CredencialesLogin>(), It.IsAny<CancellationToken>()))
            .Callback(() => ordenEjecucion.Add(orden))
            .ReturnsAsync(exitoso ? ResultadoValidacion.Ok() : ResultadoValidacion.Fallo(msg ?? "fallo"));
        return mock;
    }
}
