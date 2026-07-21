using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Autenticacion;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Infrastructure.Autenticacion.Pasos
{
    public class ValidarPermisoStep : IPasoValidacionLogin
    {
        public int Orden => 6;
        private const string CodAccionIngreso = "2000";

        public async Task<ResultadoValidacion> ValidarAsync(IDbConnection conexion, CredencialesLogin credenciales, CancellationToken ct)
        {
            using var cmd = (OracleCommand)conexion.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM USUARIOS_PRIVS WHERE COD_ACCION= :codAccion AND USUARIO =:usuario";
            cmd.Parameters.Add(new OracleParameter("codAccion", CodAccionIngreso));
            cmd.Parameters.Add(new OracleParameter("usuario", credenciales.Usuario.ToUpper()));

            var tienePermiso = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct)) > 0;

            return tienePermiso
                ? ResultadoValidacion.Ok()
                : ResultadoValidacion.Fallo("El usuario no tiene permisos para ingresar");
        }
    }
}
