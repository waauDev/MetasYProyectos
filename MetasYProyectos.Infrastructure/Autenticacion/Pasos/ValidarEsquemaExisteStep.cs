using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MetasYProyectos.Infrastructure.Autenticacion.Pasos
{
    public sealed class ValidarEsquemaExisteStep : IPasoValidacionLogin
    {
        public int Orden => 1;

        public async Task<ResultadoValidacion> ValidarAsync(IDbConnection conexion, CredencialesLogin credenciales, CancellationToken ct)
        {
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM DBA_USERS WHERE USERNAME = :esquema";
            cmd.Parameters.Add(new OracleParameter("esquema", credenciales.vigencia.ToUpperInvariant()));

            return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct)) > 0
                ? ResultadoValidacion.Ok()
                : ResultadoValidacion.Fallo("El esquema o vigencia no existe");
        }
    }
}
