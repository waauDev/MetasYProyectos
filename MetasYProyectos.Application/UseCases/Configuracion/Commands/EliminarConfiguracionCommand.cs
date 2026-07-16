using MetasYProyectos.Application.Common.Mediator;

namespace MetasYProyectos.Application.UseCases.Configuracion.Commands
{
    public sealed record EliminarConfiguracionCommand(
        string Nombre) : IRequest<EliminarConfiguracionResult>;

    public sealed record EliminarConfiguracionResult(
        bool Exitoso,
        string Mensaje);
}
