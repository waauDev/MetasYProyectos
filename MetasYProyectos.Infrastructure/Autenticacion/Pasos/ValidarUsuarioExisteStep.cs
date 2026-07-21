using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Infrastructure.Autenticacion.Pasos
{
    public class ValidarUsuarioExisteStep : IPasoValidacionLogin
    {
        public int Orden => 2;

        public async Task<ResultadoValidacion> ValidarAsync(IDbConnection conexion, CredencialesLogin c, CancellationToken ct)
        {
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM ALL_USERS WHERE USERNAME = :usuario";
            cmd.Parameters.Add(new OracleParameter("usuario", c.Usuario.ToUpper()));

            var existe = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct)) > 0;

            return existe
                ? ResultadoValidacion.Ok()
                : ResultadoValidacion.Fallo("El usuario no existe");
        }
    }
}
