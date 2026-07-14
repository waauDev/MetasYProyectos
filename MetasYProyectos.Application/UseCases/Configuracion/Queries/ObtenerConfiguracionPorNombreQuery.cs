using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed record ObtenerConfiguracionPorNombreQuery(
        string Nombre) : IRequest<ConfiguracionBDDto?>;
}
