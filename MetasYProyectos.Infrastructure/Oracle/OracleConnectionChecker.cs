using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Enums;
using MetasYProyectos.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Infrastructure.Oracle
{
    public sealed class OracleConnectionChecker : IOracleConnectionChecker
    {
        public async Task<(bool exitoso, string mensaje)> VerificarAsync(
            ConfiguracionBD configuracion,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var connectionString = ConstruirConnectionString(configuracion);

                await using var conexion = new OracleConnection(connectionString);
                await conexion.OpenAsync(cancellationToken);

                await using var comando = conexion.CreateCommand();
                comando.CommandText = "SELECT  BANNER FROM V$VERSION WHERE ROWNUM=1";

                var resultado = await comando.ExecuteScalarAsync(cancellationToken);

                var versionInfo = resultado?.ToString() ?? "desconocida";

                return (true, $"Conexion exitosa. Version detectada: {versionInfo}");
            }
            catch (OracleException ex)
            {
                return (false, $"Error Oracle [{ex.Number}]:{ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Error al conectar {ex.Message}");
            }
        }
        

        private static string ConstruirConnectionString(ConfiguracionBD config)
        {
            return config.TipoConexion switch
            {

                TipoConexion.ServiceName =>
                    $"Data Source=(DESCRIPTION=" +
                    $"(ADDRESS=(PROTOCOL=TCP)(HOST={config.Servidor})(PORT={config.Puerto}))" +
                    $"(CONNECT_DATA=(SERVICE_NAME={config.Servicio})));" +
                    $"User Id={config.Usuario};Password={config.Password};",

                TipoConexion.SID =>
                    $"Data Source=(DESCRIPTION=" +
                    $"(ADDRESS=(PROTOCOL=TCP)(HOST={config.Servidor})(PORT={config.Puerto}))" +
                    $"(CONNECT_DATA=(SID={config.Servicio})));" +
                    $"User Id={config.Usuario};Password={config.Password};",

                TipoConexion.TNS =>
                    $"Data Source={config.Servicio};" +
                    $"User Id={config.Usuario};Password={config.Password};",
                _ => throw new ArgumentOutOfRangeException(
                    nameof(config.TipoConexion),
                    "Tipo de conexion no soportado")
            };

        }
    }
}
