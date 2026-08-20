using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Application.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace MetasYProyectos.Infrastructure.Autenticacion
{
    public sealed class ValidadorPermisoAccion : IValidadorPermisoAccion
    {
        private readonly IConexionUsuarioActual _conexionActual;

        public ValidadorPermisoAccion(IConexionUsuarioActual conexionActual)
            => _conexionActual = conexionActual;

        public async Task<bool> TienePermisoAsync(string codAccion, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = "SELECT COUNT(*) FROM USUARIOS_PRIVS WHERE COD_ACCION = :codAccion AND USUARIO = :usuario";
            cmd.Parameters.Add(new OracleParameter("codAccion", codAccion));
            cmd.Parameters.Add(new OracleParameter("usuario", _conexionActual.Usuario.ToUpperInvariant()));

            return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct)) > 0;
        }
    }
}
