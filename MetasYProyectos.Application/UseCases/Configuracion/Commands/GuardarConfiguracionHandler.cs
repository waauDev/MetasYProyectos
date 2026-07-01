using FluentValidation;
using MediatR;
using MetasYProyectos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.UseCases.Configuracion.Commands
{
    public sealed class GuardarConfiguracionHandler : IRequestHandler<GuardarConfiguracionCommand, GuardarConfiguracionResult>
    {
        private readonly IConfiguracionService _servicio;
        private readonly IValidator<Application.DTOs.ConfiguracionBDDto> _validator;

        public GuardarConfiguracionHandler(
            IConfiguracionService servicio,
            IValidator<Application.DTOs.ConfiguracionBDDto> validator
            )
        {
            _servicio = servicio;
            _validator = validator;
        }

        public async Task<GuardarConfiguracionResult> Handle(
            GuardarConfiguracionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var validacion = await _validator.ValidateAsync(
                    request.Datos, cancellationToken);

                if (!validacion.IsValid)
                {
                    var errores = string.Join("|",
                        validacion.Errors.Select(e => e.ErrorMessage));
                    return new GuardarConfiguracionResult(false, errores);
                }

                var dto = request.Datos;
                if (dto.Password == "••••••••" && _servicio.Existe())
                {
                    var actual = _servicio.Obtener();
                    if (actual is not null)
                        dto = dto with { Password = actual.Password };
                }

                _servicio.Guardar(dto);

                return new GuardarConfiguracionResult(
                    true, "Configuracion Guardada Correctamente");
            }
            catch (Exception ex)
            {
                return new GuardarConfiguracionResult(
                    false, $"Error inesperado: {ex.Message}");
            }

        }
    }
}
