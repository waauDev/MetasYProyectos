using MediatR;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.UseCases.Configuracion.Queries
{
    public sealed class ObtenerConfiguracionHandler: IRequestHandler<ObtenerConfiguracionQuery, ConfiguracionBDDto?>
    {
        private readonly IConfiguracionService _servicio;

        public ObtenerConfiguracionHandler(IConfiguracionService servicio)
            =>_servicio= servicio;

        public Task<ConfiguracionBDDto?> Handle(
            ObtenerConfiguracionQuery query,
            CancellationToken cancellationToken)
        {
            var dto = _servicio.Obtener();
            return Task.FromResult(dto);
        }
    }
}
