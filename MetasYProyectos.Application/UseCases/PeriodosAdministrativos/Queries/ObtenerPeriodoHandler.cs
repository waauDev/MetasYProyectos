using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed class ObtenerPeriodoHandler : IRequestHandler<ObtenerPeriodoQuery, PeriodoAdministrativoDto?>
    {
        private readonly IPeriodoAdministrativoRepository _repositorio;

        public ObtenerPeriodoHandler(IPeriodoAdministrativoRepository repositorio)
            => _repositorio = repositorio;

        public async Task<PeriodoAdministrativoDto?> Handle(ObtenerPeriodoQuery request, CancellationToken ct)
        {
            var entidad = await _repositorio.ObtenerAsync(request.IdPeriodo, ct);
            return entidad is null ? null : PeriodoAdministrativoMapper.ToDto(entidad);
        }
    }
}
