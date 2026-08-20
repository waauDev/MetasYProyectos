using FluentValidation;
using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.Validators
{
    public sealed class PeriodoAdministrativoDtoValidator : AbstractValidator<PeriodoAdministrativoDto>
    {
        public PeriodoAdministrativoDtoValidator()
        {
            RuleFor(x => x.IdPeriodo)
                .GreaterThan(0)
                    .WithMessage("El código del periodo es obligatorio");

            RuleFor(x => x.NitGobernante)
                .NotEmpty()
                    .WithMessage("El NIT del gobernante es obligatorio")
                .MaximumLength(20)
                    .WithMessage("El NIT no puede superar los 20 caracteres");

            RuleFor(x => x.FechaInicio)
                .NotEqual(default(DateTime))
                    .WithMessage("La fecha inicial es obligatoria");

            RuleFor(x => x.FechaFinal)
                .NotEqual(default(DateTime))
                    .WithMessage("La fecha final es obligatoria")
                .GreaterThanOrEqualTo(x => x.FechaInicio)
                    .WithMessage("La fecha final no puede ser anterior a la fecha inicial");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0)
                    .WithMessage("La cantidad debe ser mayor a cero");
        }
    }
}
