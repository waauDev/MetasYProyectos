
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
            System.Data.IDbConnection conexion;
            try
            {
                conexion = await _connectionFactory.CrearConexionAsync(request.Credenciales, ct);


            }catch(CredencialesInvalidasException ex) 
            {
                return LoginResult.Fallo(ex.Message);
            }
            catch (ConexionBaseDatosException )
            {
                return LoginResult.Fallo("No fue posible conectar con la base de datos");
            }

            using (conexion)
            {
                foreach (var paso in _pasos)
                {
                    var resultado = await paso.ValidarAsync(conexion, request.Credenciales, ct);
                    if (!resultado.Existoso)
                        return LoginResult.Fallo(resultado.MensajeError!);
                }
            }

            return LoginResult.Ok(request.Credenciales);
        }
    }
}
