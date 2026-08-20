using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Commands
{
    public sealed class EliminarPeriodoHandler : IRequestHandler<EliminarPeriodoCommand, PeriodoAdministrativoResult>
    {
        private readonly IPeriodoAdministrativoRepository _repositorio;

        public EliminarPeriodoHandler(IPeriodoAdministrativoRepository repositorio)
            => _repositorio = repositorio;

        public async Task<PeriodoAdministrativoResult> Handle(EliminarPeriodoCommand request, CancellationToken ct)
        {
            try
            {
                if (await _repositorio.ObtenerAsync(request.IdPeriodo, ct) is null)
                    return new PeriodoAdministrativoResult(false, "El periodo ya no existe");

                await _repositorio.EliminarAsync(request.IdPeriodo, ct);

                return new PeriodoAdministrativoResult(true, "Periodo eliminado correctamente");
            }
            catch (Exception ex)
            {
                return new PeriodoAdministrativoResult(false, $"No fue posible eliminar el periodo: {ex.Message}");
            }
        }
    }
}
