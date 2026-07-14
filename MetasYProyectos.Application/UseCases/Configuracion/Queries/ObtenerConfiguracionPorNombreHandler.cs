using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Interfaces;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed class ObtenerConfiguracionPorNombreHandler : IRequestHandler<ObtenerConfiguracionPorNombreQuery, ConfiguracionBDDto?>
    {
        private readonly IConfiguracionService _servicio;

        public ObtenerConfiguracionPorNombreHandler(IConfiguracionService servicio)
            => _servicio = servicio;

        public Task<ConfiguracionBDDto?> Handle(
            ObtenerConfiguracionPorNombreQuery query,
            CancellationToken cancellationToken)
        {
            var dto = _servicio.ObtenerPorNombre(query.Nombre);
            return Task.FromResult(dto);
        }
    }
}
