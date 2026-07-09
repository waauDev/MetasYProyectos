using MediatR;
using MetasYProyectos.Domain.Autenticacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion
{
    public record AutenticarUsuarioCommand(CredencialesLogin Credenciales) : IRequest<LoginResult>;
}
