using FluentValidation;
using MetasYProyectos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Validators
{
   public sealed class ConfiguracionBDDtoValidator: AbstractValidator<ConfiguracionBDDto>
    {
        public ConfiguracionBDDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                    .WithMessage("El nombre de la configuración es obligatorio")
                .MaximumLength(100)
                    .WithMessage("El nombre no puede superar los 100 caracteres");

            RuleFor(x => x.Servidor)
                .NotEmpty()
                    .WithMessage("El servidor o host es obligatorio")
                .MaximumLength(255)
                    .WithMessage("El servidor no puede superar los 255 caracteres");

            RuleFor(x=>x.Puerto)
                .NotEmpty()
                    .WithMessage("El puerto es obligatorio")
                .Must(SerPuertoValido)
                    .WithMessage("El puerto debe ser un numero entre 1 y 65535");

            RuleFor(x=> x.Servicio)
                .NotEmpty()
                    .WithMessage("El puerto es obligatorio")
                 .MaximumLength(255)
                    .WithMessage("El servidor no puede superar los 255 caracteres");
            
            RuleFor(x => x.Usuario)
                .NotEmpty()
                    .WithMessage("El puerto es obligatorio")
                 .MaximumLength(255)
                    .WithMessage("El usuario no puede superar los 255 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("El puerto es obligatorio")
                 .MinimumLength(4)
                    .WithMessage("la contraseña debe tener al menos 4 caracteres");

            RuleFor(x => x.Version)
                .IsInEnum()
                    .WithMessage("La version de oracle seleccionada no es Valida");

            RuleFor(x=> x.TipoConexion)
                .IsInEnum()
                    .WithMessage("El tipo de conexion no es valido");




        }

        private static bool SerPuertoValido(string puerto)
            => int.TryParse(puerto, out var n) && n >= 1 && n <= 65535;
    }
}
