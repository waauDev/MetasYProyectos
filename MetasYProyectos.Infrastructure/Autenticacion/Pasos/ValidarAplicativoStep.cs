using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Infrastructure.Autenticacion.Pasos
{
    public class ValidarAplicativoStep : IPasoValidacionLogin
    {
        public int Orden => 4;

        private const string NombreAplicativo = "METASPROYECTOS";

        public async Task<ResultadoValidacion> ValidarAsync(IDbConnection conexion, CredencialesLogin credenciales, CancellationToken ct)
        {
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.CommandText = "SELECT  VER_APP FROM CTRL_APP WHERE NOM_APP=:nomApp";
            cmd.Parameters.Add(new OracleParameter("nomApp", NombreAplicativo));

            var version = await cmd.ExecuteScalarAsync(ct);

            return version != null
                ? ResultadoValidacion.Ok()
                : ResultadoValidacion.Fallo($"Acceso denegado. No cuenta con una licencia para utilizar {NombreAplicativo}. Comuníquese con el área de soporte");
        }
    }
}
