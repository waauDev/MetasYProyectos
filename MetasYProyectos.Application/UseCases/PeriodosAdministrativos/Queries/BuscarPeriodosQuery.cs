using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed record BuscarPeriodosQuery(
        int? IdPeriodoInicial,
        int? IdPeriodoFinal,
        string? NitGobernante,
        int Pagina) : IRequest<BuscarPeriodosResultado>;

    public sealed record BuscarPeriodosResultado(
        List<PeriodoAdministrativoDto> Items,
        int Total,
        int Pagina,
        int TamanoPagina);
}
