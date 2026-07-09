
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed record ObtenerConfiguracionQuery : IRequest<ConfiguracionBDDto?>;
  
}
