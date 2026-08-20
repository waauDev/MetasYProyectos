using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed record CrearPeriodoCommand(PeriodoAdministrativoDto Datos) : IRequest<PeriodoAdministrativoResult>;

    public sealed record PeriodoAdministrativoResult(bool Exitoso, string Mensaje);
}
