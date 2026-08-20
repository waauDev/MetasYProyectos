using FluentValidation;
using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Exceptions;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed class ActualizarPeriodoHandler : IRequestHandler<ActualizarPeriodoCommand, PeriodoAdministrativoResult>
    {
        private readonly IPeriodoAdministrativoRepository _repositorio;
        private readonly INitRepository _nitRepositorio;
        private readonly IValidator<PeriodoAdministrativoDto> _validador;

        public ActualizarPeriodoHandler(
            IPeriodoAdministrativoRepository repositorio,
            INitRepository nitRepositorio,
            IValidator<PeriodoAdministrativoDto> validador)
        {
            _repositorio = repositorio;
            _nitRepositorio = nitRepositorio;
            _validador = validador;
        }

        public async Task<PeriodoAdministrativoResult> Handle(ActualizarPeriodoCommand request, CancellationToken ct)
        {
            var validacion = await _validador.ValidateAsync(request.Datos, ct);
            if (!validacion.IsValid)
                return new PeriodoAdministrativoResult(false, string.Join("|", validacion.Errors.Select(e => e.ErrorMessage)));

            try
            {
                if (await _repositorio.ObtenerAsync(request.Datos.IdPeriodo, ct) is null)
                    return new PeriodoAdministrativoResult(false, "El periodo ya no existe");

                if (await _nitRepositorio.ObtenerAsync(request.Datos.NitGobernante, ct) is null)
                    return new PeriodoAdministrativoResult(false, "El NIT del gobernante no existe");

                var entidad = PeriodoAdministrativoMapper.ToEntity(request.Datos);
                await _repositorio.ActualizarAsync(entidad, ct);

                return new PeriodoAdministrativoResult(true, "Periodo actualizado correctamente");
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
