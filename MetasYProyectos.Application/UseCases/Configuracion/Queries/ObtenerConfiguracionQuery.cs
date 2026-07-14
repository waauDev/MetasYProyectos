using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed record ObtenerConfiguracionQuery : IRequest<List<ConfiguracionBDDto>>;
}
