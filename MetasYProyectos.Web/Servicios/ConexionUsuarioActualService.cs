using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using MetasYProyectos.Web.Autenticacion;
using Microsoft.AspNetCore.DataProtection;
using System.Data;
using System.Security.Claims;

namespace MetasYProyectos.Web.Servicios
{
    // ponytail: abre una conexión Oracle nueva en cada llamada; agregar caché por-request si el tráfico lo justifica.
    public sealed class ConexionUsuarioActualService : IConexionUsuarioActual
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOracleConnectionFactory _connectionFactory;
        private readonly IDataProtector _protector;

        public ConexionUsuarioActualService(
            IHttpContextAccessor httpContextAccessor,
            IOracleConnectionFactory connectionFactory,
            IDataProtectionProvider dataProtectionProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionFactory = connectionFactory;
            _protector = dataProtectionProvider.CreateProtector(EsquemasAutenticacion.PropositoProteccionSesion);
        }

        public string Usuario => ObtenerClaim(ClaimTypes.Name);

        public Task<IDbConnection> AbrirAsync(CancellationToken ct)
        {
            var credenciales = new CredencialesLogin(
                Usuario,
                _protector.Unprotect(ObtenerClaim(EsquemasAutenticacion.ClaimPasswordCifrado)),
                ObtenerClaim(EsquemasAutenticacion.ClaimVigencia),
                ObtenerClaim(EsquemasAutenticacion.ClaimBaseDatos));

            return _connectionFactory.CrearConexionUsuarioAsync(credenciales, ct);
        }

        private string ObtenerClaim(string tipo)
            => _httpContextAccessor.HttpContext?.User.FindFirstValue(tipo)
                ?? throw new InvalidOperationException("No hay una sesión de usuario activa");
    }
}
