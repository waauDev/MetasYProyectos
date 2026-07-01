using MetasYProyectos.Domain.Enums;
using MetasYProyectos.Domain.Exceptions;

namespace MetasYProyectos.Domain.Entities;

public class ConfiguracionBD
{
    public string Servidor { get; private set; } = string.Empty;
    public string Puerto { get; private set; } = "1521";
    public string Servicio { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Usuario { get; private set; } = string.Empty;

    public VersionOracle Version { get; private set; } = VersionOracle.Oracle19;
    public TipoConexion TipoConexion { get; private set; } = TipoConexion.ServiceName;

    private ConfiguracionBD() { }

    public static ConfiguracionBD Crear(
        string servidor,
        string puerto,
        string servicio,
        string usuario,
        string password,
        VersionOracle version,
        TipoConexion tipoConexion
        )
    {

        if (string.IsNullOrWhiteSpace(servidor))
            throw new ConfiguracionInvalidaException(
                nameof(Servidor),
                "El servidor o host es obligatorio");
        if (string.IsNullOrWhiteSpace(usuario))
            throw new ConfiguracionInvalidaException(
                nameof(Usuario),
                "El usuario de la base de datos es obligatorio");

        if (string.IsNullOrWhiteSpace(password))
            throw new ConfiguracionInvalidaException(
                nameof(password),
                "La contraseña es obligatoria");


        if (tipoConexion != TipoConexion.TNS
            && string.IsNullOrEmpty(servicio))
            throw new ConfiguracionInvalidaException(
                nameof(servicio),
                "El service Name o SID es obligatorio");

        if (!string.IsNullOrWhiteSpace(puerto))
        {
            if (!int.TryParse(puerto, out int puertoNum)
                || puertoNum < 1
                || puertoNum > 65535)
                throw new ConfiguracionInvalidaException(
                    nameof(Puerto),
                    "El puerto debe ser entre 1 y 65535");
        }

        return new ConfiguracionBD
        {
            Servidor = servidor.Trim(),
            Puerto = string.IsNullOrWhiteSpace(puerto) ? "1521" : puerto.Trim(),
            Servicio = servicio.Trim(),
            Usuario = usuario.Trim(),
            Password = password,
            Version = version,
            TipoConexion = tipoConexion

        };

    }

    public bool usaTNS() => TipoConexion == TipoConexion.TNS;

    public String DescripcionSegura()
        => $"Servidor={Servidor}:{Puerto}|" +
        $"Tipo={TipoConexion}| " +
        $"Oracle {(int)Version}|" +
        $"Usuario= {Usuario}";

    public ConfiguracionBD ConNuevoPassword(string nuevoPassword)
        =>Crear(Servidor,Puerto, Servicio, Usuario, nuevoPassword,Version,TipoConexion);
}
        

