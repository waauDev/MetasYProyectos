using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace MetasYProyectos.Infrastructure.Persistence
{
    public sealed class NitRepository : INitRepository
    {
        private readonly IConexionUsuarioActual _conexionActual;

        public NitRepository(IConexionUsuarioActual conexionActual)
            => _conexionActual = conexionActual;

        public async Task<Nit?> ObtenerAsync(string numero, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = "SELECT NIT, NOMBRE FROM NIT WHERE NIT = :numero";
            cmd.Parameters.Add(new OracleParameter("numero", numero));

            using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct))
                return null;

            return new Nit(reader["NIT"].ToString() ?? string.Empty, reader["NOMBRE"].ToString() ?? string.Empty);
        }

        public async Task<List<Nit>> BuscarAsync(string texto, int maximo, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = @"
                SELECT NIT, NOMBRE FROM NIT
                WHERE (UPPER(NIT) LIKE UPPER(:texto) OR UPPER(NOMBRE) LIKE UPPER(:texto))
                  AND ROWNUM <= :maximo
                ORDER BY NOMBRE";
            cmd.Parameters.Add(new OracleParameter("texto", $"%{texto}%"));
            cmd.Parameters.Add(new OracleParameter("maximo", maximo));

            var resultado = new List<Nit>();
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
                resultado.Add(new Nit(reader["NIT"].ToString() ?? string.Empty, reader["NOMBRE"].ToString() ?? string.Empty));

            return resultado;
        }
    }
}
