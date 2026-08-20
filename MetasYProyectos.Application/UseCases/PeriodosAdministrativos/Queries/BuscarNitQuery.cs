using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed record BuscarNitQuery(string Texto) : IRequest<List<NitDto>>;
}
