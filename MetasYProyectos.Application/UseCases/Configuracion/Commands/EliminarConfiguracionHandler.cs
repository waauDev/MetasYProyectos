using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.Interfaces;

namespace MetasYProyectos.Application.UseCases.Configuracion.Commands
{
    public sealed class EliminarConfiguracionHandler : IRequestHandler<EliminarConfiguracionCommand, EliminarConfiguracionResult>
    {
        private readonly IConfiguracionService _servicio;

        public EliminarConfiguracionHandler(IConfiguracionService servicio)
            => _servicio = servicio;

        public Task<EliminarConfiguracionResult> Handle(
            EliminarConfiguracionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _servicio.Eliminar(request.Nombre);
                return Task.FromResult(new EliminarConfiguracionResult(true, "Configuración eliminada correctamente"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new EliminarConfiguracionResult(false, $"Error al eliminar: {ex.Message}"));
            }
        }
    }
}
