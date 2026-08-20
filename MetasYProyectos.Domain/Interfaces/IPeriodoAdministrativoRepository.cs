using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Filtros;

namespace MetasYProyectos.Domain.Interfaces
{
    public interface IPeriodoAdministrativoRepository
    {
        Task<(List<PeriodoAdministrativo> Items, int Total)> BuscarAsync(
            PeriodoAdministrativoFiltro filtro, int pagina, int tamanoPagina, CancellationToken ct);

        Task<PeriodoAdministrativo?> ObtenerAsync(int idPeriodo, CancellationToken ct);

        Task CrearAsync(PeriodoAdministrativo periodo, CancellationToken ct);

        Task ActualizarAsync(PeriodoAdministrativo periodo, CancellationToken ct);

        Task EliminarAsync(int idPeriodo, CancellationToken ct);
    }
}
