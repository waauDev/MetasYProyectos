
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.UseCases.Configuracion.Commands
{
    public sealed record GuardarConfiguracionCommand(
        ConfiguracionBDDto Datos): IRequest<GuardarConfiguracionResult>;

    public sealed record GuardarConfiguracionResult(
        bool Exitoso,
        string Mensaje);
}
