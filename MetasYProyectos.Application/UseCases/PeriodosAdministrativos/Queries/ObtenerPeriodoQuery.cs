using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed record ObtenerPeriodoQuery(int IdPeriodo) : IRequest<PeriodoAdministrativoDto?>;
}
