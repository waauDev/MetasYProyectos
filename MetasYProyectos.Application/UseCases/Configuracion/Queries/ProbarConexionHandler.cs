
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.Interfaces;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed class ProbarConexionHandler : IRequestHandler<ProbarConexionQuery, ProbarConexionResult>
    {
        private readonly IConfiguracionService _servicio;
        public ProbarConexionHandler(IConfiguracionService servicio)
            => _servicio = servicio;
        public async Task<ProbarConexionResult> Handle(ProbarConexionQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var dto = query.Datos;

                if(dto.Password== "••••••••" && _servicio.Existe())
                {
                    var actual = _servicio.Obtener();
                    if (actual is not null)
                        dto = dto with { Password = actual.Password };
                }

                var (ok, mensaje) = await _servicio.ProbarConexionAsync(dto, cancellationToken);

                return new ProbarConexionResult(ok, mensaje);
            }catch(Exception ex)
            {
                return new ProbarConexionResult(false,
                    $"Error inesperado al realizar conexion:{ex.Message}");
            }

        }
    }

}
