using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Filtros;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed class BuscarPeriodosHandler : IRequestHandler<BuscarPeriodosQuery, BuscarPeriodosResultado>
    {
        public const int TamanoPagina = 15;

        private readonly IPeriodoAdministrativoRepository _repositorio;

        public BuscarPeriodosHandler(IPeriodoAdministrativoRepository repositorio)
            => _repositorio = repositorio;

        public async Task<BuscarPeriodosResultado> Handle(BuscarPeriodosQuery request, CancellationToken ct)
        {
            var filtro = new PeriodoAdministrativoFiltro(
                request.IdPeriodoInicial, request.IdPeriodoFinal, request.NitGobernante);

            var pagina = request.Pagina < 1 ? 1 : request.Pagina;

            var (items, total) = await _repositorio.BuscarAsync(filtro, pagina, TamanoPagina, ct);

            return new BuscarPeriodosResultado(
                items.Select(PeriodoAdministrativoMapper.ToDto).ToList(),
                total,
                pagina,
                TamanoPagina);
        }
    }
}
