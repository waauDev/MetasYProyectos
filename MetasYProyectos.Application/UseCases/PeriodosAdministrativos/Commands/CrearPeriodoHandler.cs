using FluentValidation;
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Exceptions;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed class CrearPeriodoHandler : IRequestHandler<CrearPeriodoCommand, PeriodoAdministrativoResult>
    {
        private readonly IPeriodoAdministrativoRepository _repositorio;
        private readonly INitRepository _nitRepositorio;
        private readonly IValidator<PeriodoAdministrativoDto> _validador;

        public CrearPeriodoHandler(
            IPeriodoAdministrativoRepository repositorio,
            INitRepository nitRepositorio,
            IValidator<PeriodoAdministrativoDto> validador)
        {
            _repositorio = repositorio;
            _nitRepositorio = nitRepositorio;
            _validador = validador;
        }

        public async Task<PeriodoAdministrativoResult> Handle(CrearPeriodoCommand request, CancellationToken ct)
        {
            var validacion = await _validador.ValidateAsync(request.Datos, ct);
            if (!validacion.IsValid)
                return new PeriodoAdministrativoResult(false, string.Join("|", validacion.Errors.Select(e => e.ErrorMessage)));

            try
            {
                if (await _repositorio.ObtenerAsync(request.Datos.IdPeriodo, ct) is not null)
                    return new PeriodoAdministrativoResult(false, "Ya existe un periodo con ese código");

                if (await _nitRepositorio.ObtenerAsync(request.Datos.NitGobernante, ct) is null)
                    return new PeriodoAdministrativoResult(false, "El NIT del gobernante no existe");

                var entidad = PeriodoAdministrativoMapper.ToEntity(request.Datos);
                await _repositorio.CrearAsync(entidad, ct);

                return new PeriodoAdministrativoResult(true, "Periodo creado correctamente");
            }
            catch (PeriodoAdministrativoInvalidoException ex)
            {
                return new PeriodoAdministrativoResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new PeriodoAdministrativoResult(false, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
