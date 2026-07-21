using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Infrastructure.Autenticacion.Pasos
{
    public class ValidarUsuarioActivoStep : IPasoValidacionLogin
    {
        public int Orden => 5;

        public async Task<ResultadoValidacion> ValidarAsync(IDbConnection conexion, CredencialesLogin credenciales, CancellationToken ct)
        {
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.CommandText = "SELECT ACTIVO FROM USUARIOS_PCT WHERE USUARIO= :usuario";
            cmd.Parameters.Add(new OracleParameter("usuario", credenciales.Usuario.ToUpper()));

            var valor = (await cmd.ExecuteScalarAsync(ct))?.ToString();

            return valor == "S"
                ? ResultadoValidacion.Ok()
                : ResultadoValidacion.Fallo("El usuario se encuentra inactivo");
        }

    }
}
