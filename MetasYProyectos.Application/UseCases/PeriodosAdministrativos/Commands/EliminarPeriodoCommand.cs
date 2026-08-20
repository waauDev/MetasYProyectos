using MetasYProyectos.Application.Common.Mediator;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed record EliminarPeriodoCommand(int IdPeriodo) : IRequest<PeriodoAdministrativoResult>;
}
