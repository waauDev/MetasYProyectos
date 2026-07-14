using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Interfaces;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed class ObtenerConfiguracionHandler : IRequestHandler<ObtenerConfiguracionQuery, List<ConfiguracionBDDto>>
    {
        private readonly IConfiguracionService _servicio;

        public ObtenerConfiguracionHandler(IConfiguracionService servicio)
            => _servicio = servicio;

        public Task<List<ConfiguracionBDDto>> Handle(
            ObtenerConfiguracionQuery query,
            CancellationToken cancellationToken)
        {
            var lista = _servicio.ObtenerTodas();
            return Task.FromResult(lista);
        }
    }
}
