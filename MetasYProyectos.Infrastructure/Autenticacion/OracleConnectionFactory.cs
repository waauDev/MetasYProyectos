using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Application.Autenticacion.Excepciones;
using MetasYProyectos.Domain.Autenticacion;
using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Enums;
using MetasYProyectos.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MetasYProyectos.Infrastructure.Autenticacion
{
    public class OracleConnectionFactory : IOracleConnectionFactory
    {
        private readonly IConfiguracionRepository _configuracionRepository;

        public OracleConnectionFactory(IConfiguracionRepository configuracionRepository)
        {
            _configuracionRepository = configuracionRepository;
        }

        public async Task<IDbConnection> CrearConexionTecnicaAsync(string baseDatos, CancellationToken ct)
        {
            var config = ObtenerConfiguracion(baseDatos);
            return await AbrirConexionAsync(config, config.Usuario, config.Password, ct);
        }

        public async Task<IDbConnection> CrearConexionUsuarioAsync(CredencialesLogin credenciales, CancellationToken ct)
        {
            var config = ObtenerConfiguracion(credenciales.BaseDatos);
            var conexion = await AbrirConexionAsync(config, credenciales.Usuario, credenciales.password, ct);

            try
            {
                var esquemaSeguro = SanearIdentificador(credenciales.vigencia);

                using var cmdSchema = conexion.CreateCommand();
                cmdSchema.CommandText = $"ALTER SESSION SET CURRENT_SCHEMA = {esquemaSeguro}";
                await cmdSchema.ExecuteNonQueryAsync(ct);

                using var cmdFecha = conexion.CreateCommand();
                cmdFecha.CommandText = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY'";
                await cmdFecha.ExecuteNonQueryAsync(ct);

                return conexion;
            }
            catch (OracleException ex)
            {
                conexion.Dispose();
                throw new ConexionBaseDatosException("No fue posible configurar la sesión de base de datos.", ex);
            }
        }

        private ConfiguracionBD ObtenerConfiguracion(string baseDatos)
            => _configuracionRepository.ObtenerPorNombre(baseDatos)
                ?? throw new ConexionBaseDatosException(
                    "No hay una configuración de base de datos guardada. Contacte al administrador.",
                    new InvalidOperationException());

        private static async Task<OracleConnection> AbrirConexionAsync(
            ConfiguracionBD config,
            string usuario,
            string password,
            CancellationToken ct)
        {
            var conexion = new OracleConnection(ConstruirCadenaConexion(config, usuario, password));

            try
            {
                await conexion.OpenAsync(ct);
                return conexion;
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                conexion.Dispose();
                throw new CredencialesInvalidasException();
            }
            catch (OracleException ex)
            {
                conexion.Dispose();
                throw new ConexionBaseDatosException("No fue posible conectar con la base de datos.", ex);
            }
        }

        private static string ConstruirCadenaConexion(ConfiguracionBD config, string usuario, string password)
        {
            var dataSource = config.TipoConexion switch
            {
                TipoConexion.ServiceName =>
                    $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={config.Servidor})(PORT={config.Puerto}))(CONNECT_DATA=(SERVICE_NAME={config.Servicio})))",
                TipoConexion.SID =>
                    $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={config.Servidor})(PORT={config.Puerto}))(CONNECT_DATA=(SID={config.Servicio})))",
                TipoConexion.TNS => config.Servicio,
                _ => throw new ArgumentOutOfRangeException(nameof(config.TipoConexion))
            };

            return $"User Id={usuario};Password={password};Data Source={dataSource};";
        }

        private static string SanearIdentificador(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || !valor.All(ch => char.IsLetterOrDigit(ch) || ch == '_'))
                throw new ConexionBaseDatosException("El esquema/vigencia especificado no es válido.", new ArgumentException(nameof(valor)));

            return valor.ToUpperInvariant();
        }
    }
}
