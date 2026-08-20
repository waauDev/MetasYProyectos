using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed record ActualizarPeriodoCommand(PeriodoAdministrativoDto Datos) : IRequest<PeriodoAdministrativoResult>;
}
