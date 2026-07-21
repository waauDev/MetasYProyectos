
using MetasYProyectos.Application.Autenticacion.Excepciones;
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Domain.Autenticacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion
{
    public class AutenticarUsuarioCommandHandler:IRequestHandler<AutenticarUsuarioCommand, LoginResult>
    {
        private readonly IOracleConnectionFactory _connectionFactory;
        private readonly IEnumerable<IPasoValidacionLogin> _pasos;

        public AutenticarUsuarioCommandHandler(
            IOracleConnectionFactory connectionFactory,
            IEnumerable<IPasoValidacionLogin> pasos)
        {
            _connectionFactory = connectionFactory;
            _pasos = pasos.OrderBy(p => p.Orden);
            
        }

        public async Task<LoginResult> Handle (AutenticarUsuarioCommand request, CancellationToken ct)
        {
            try
            {
                using var conexionTecnica = await _connectionFactory.CrearConexionTecnicaAsync(
                    request.Credenciales.BaseDatos, ct);

                foreach (var paso in _pasos.Where(p => p.Orden <= 2))
                {
                    var resultado = await paso.ValidarAsync(conexionTecnica, request.Credenciales, ct);
                    if (!resultado.Existoso)
                        return LoginResult.Fallo(resultado.MensajeError!);
                }

                using var conexionUsuario = await _connectionFactory.CrearConexionUsuarioAsync(
                    request.Credenciales, ct);

                foreach (var paso in _pasos.Where(p => p.Orden > 2))
                {
                    var resultado = await paso.ValidarAsync(conexionUsuario, request.Credenciales, ct);
                    if (!resultado.Existoso)
                        return LoginResult.Fallo(resultado.MensajeError!);
                }

                return LoginResult.Ok(request.Credenciales);
            }
            catch (CredencialesInvalidasException ex)
            {
                return LoginResult.Fallo(ex.Message);
            }
            catch (ConexionBaseDatosException ex)
            {
                return LoginResult.Fallo(ex.Message);
            }
            catch (Exception)
            {
                return LoginResult.Fallo("No fue posible validar el acceso a la base de datos");
            }
        }
    }
}
