using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Filtros;
using MetasYProyectos.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MetasYProyectos.Infrastructure.Persistence
{
    public sealed class PeriodoAdministrativoRepository : IPeriodoAdministrativoRepository
    {
        private readonly IConexionUsuarioActual _conexionActual;

        public PeriodoAdministrativoRepository(IConexionUsuarioActual conexionActual)
            => _conexionActual = conexionActual;

        public async Task<(List<PeriodoAdministrativo> Items, int Total)> BuscarAsync(
            PeriodoAdministrativoFiltro filtro, int pagina, int tamanoPagina, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);

            var total = await ContarAsync(conexion, filtro, ct);

            using var cmd = CrearComando(conexion, @"
                SELECT A.ID_MYP_PERIODO_ADM, A.ID_COM_ENTIDAD, A.NIT_GOBERNANTE,
                       A.FECHA_INICIO, A.FECHA_FINAL, A.CANTIDAD, B.NOMBRE
                FROM MYP_PERIODO_ADM A
                JOIN NIT B ON RTRIM(LTRIM(A.NIT_GOBERNANTE)) = B.NIT
                WHERE A.ID_MYP_PERIODO_ADM <> 0
                  AND (:idIni IS NULL OR A.ID_MYP_PERIODO_ADM >= :idIni)
                  AND (:idFin IS NULL OR A.ID_MYP_PERIODO_ADM <= :idFin)
                  AND (:nit IS NULL OR A.NIT_GOBERNANTE = :nit)
                ORDER BY A.ID_MYP_PERIODO_ADM
                OFFSET :offset ROWS FETCH NEXT :tamanoPagina ROWS ONLY");

            AgregarFiltro(cmd, filtro);
            cmd.Parameters.Add(new OracleParameter("offset", (pagina - 1) * tamanoPagina));
            cmd.Parameters.Add(new OracleParameter("tamanoPagina", tamanoPagina));

            var items = new List<PeriodoAdministrativo>();
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
                items.Add(Leer(reader));

            return (items, total);
        }

        private static async Task<int> ContarAsync(IDbConnection conexion, PeriodoAdministrativoFiltro filtro, CancellationToken ct)
        {
            using var cmd = CrearComando(conexion, @"
                SELECT COUNT(*)
                FROM MYP_PERIODO_ADM A
                JOIN NIT B ON RTRIM(LTRIM(A.NIT_GOBERNANTE)) = B.NIT
                WHERE A.ID_MYP_PERIODO_ADM <> 0
                  AND (:idIni IS NULL OR A.ID_MYP_PERIODO_ADM >= :idIni)
                  AND (:idFin IS NULL OR A.ID_MYP_PERIODO_ADM <= :idFin)
                  AND (:nit IS NULL OR A.NIT_GOBERNANTE = :nit)");

            AgregarFiltro(cmd, filtro);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
        }

        public async Task<PeriodoAdministrativo?> ObtenerAsync(int idPeriodo, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = CrearComando(conexion, @"
                SELECT A.ID_MYP_PERIODO_ADM, A.ID_COM_ENTIDAD, A.NIT_GOBERNANTE,
                       A.FECHA_INICIO, A.FECHA_FINAL, A.CANTIDAD, B.NOMBRE
                FROM MYP_PERIODO_ADM A
                JOIN NIT B ON RTRIM(LTRIM(A.NIT_GOBERNANTE)) = B.NIT
                WHERE A.ID_MYP_PERIODO_ADM = :id");
            cmd.Parameters.Add(new OracleParameter("id", idPeriodo));

            using var reader = await cmd.ExecuteReaderAsync(ct);
            return await reader.ReadAsync(ct) ? Leer(reader) : null;
        }

        public async Task CrearAsync(PeriodoAdministrativo periodo, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = CrearComando(conexion, @"
                INSERT INTO MYP_PERIODO_ADM
                    (ID_MYP_PERIODO_ADM, ID_COM_ENTIDAD, NIT_GOBERNANTE, FECHA_INICIO, FECHA_FINAL, CANTIDAD)
                VALUES
                    (:id, :idComEntidad, :nit, :fechaInicio, :fechaFinal, :cantidad)");
            AgregarParametros(cmd, periodo);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task ActualizarAsync(PeriodoAdministrativo periodo, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = CrearComando(conexion, @"
                UPDATE MYP_PERIODO_ADM
                SET ID_COM_ENTIDAD = :idComEntidad,
                    NIT_GOBERNANTE = :nit,
                    FECHA_INICIO = :fechaInicio,
                    FECHA_FINAL = :fechaFinal,
                    CANTIDAD = :cantidad
                WHERE ID_MYP_PERIODO_ADM = :id");
            AgregarParametros(cmd, periodo);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task EliminarAsync(int idPeriodo, CancellationToken ct)
        {
            using var conexion = await _conexionActual.AbrirAsync(ct);
            using var cmd = CrearComando(conexion, "DELETE FROM MYP_PERIODO_ADM WHERE ID_MYP_PERIODO_ADM = :id");
            cmd.Parameters.Add(new OracleParameter("id", idPeriodo));
            await cmd.ExecuteNonQueryAsync(ct);
        }

        private static OracleCommand CrearComando(IDbConnection conexion, string sql)
        {
            var cmd = (OracleCommand)conexion.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = sql;
            return cmd;
        }

        private static void AgregarFiltro(OracleCommand cmd, PeriodoAdministrativoFiltro filtro)
        {
            cmd.Parameters.Add(new OracleParameter("idIni", OracleDbType.Int32) { Value = (object?)filtro.IdPeriodoInicial ?? DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("idFin", OracleDbType.Int32) { Value = (object?)filtro.IdPeriodoFinal ?? DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("nit", OracleDbType.Varchar2) { Value = (object?)filtro.NitGobernante ?? DBNull.Value });
        }

        private static void AgregarParametros(OracleCommand cmd, PeriodoAdministrativo periodo)
        {
            cmd.Parameters.Add(new OracleParameter("id", periodo.IdPeriodo));
            cmd.Parameters.Add(new OracleParameter("idComEntidad", periodo.IdComEntidad));
            cmd.Parameters.Add(new OracleParameter("nit", periodo.NitGobernante));
            cmd.Parameters.Add(new OracleParameter("fechaInicio", periodo.FechaInicio));
            cmd.Parameters.Add(new OracleParameter("fechaFinal", periodo.FechaFinal));
            cmd.Parameters.Add(new OracleParameter("cantidad", periodo.Cantidad));
        }

        private static PeriodoAdministrativo Leer(IDataRecord reader)
            => PeriodoAdministrativo.Crear(
                Convert.ToInt32(reader["ID_MYP_PERIODO_ADM"]),
                Convert.ToInt32(reader["ID_COM_ENTIDAD"]),
                reader["NIT_GOBERNANTE"].ToString() ?? string.Empty,
                Convert.ToDateTime(reader["FECHA_INICIO"]),
                Convert.ToDateTime(reader["FECHA_FINAL"]),
                Convert.ToInt32(reader["CANTIDAD"]),
                reader["NOMBRE"]?.ToString());
    }
}
