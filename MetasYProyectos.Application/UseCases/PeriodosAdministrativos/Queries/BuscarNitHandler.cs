using MetasYProyectos.Application.Common.Mediator;
using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Application.UseCases.PeriodosAdministrativos.Queries
{
    public sealed class BuscarNitHandler : IRequestHandler<BuscarNitQuery, List<NitDto>>
    {
        private readonly INitRepository _repositorio;

        public BuscarNitHandler(INitRepository repositorio)
            => _repositorio = repositorio;

        public async Task<List<NitDto>> Handle(BuscarNitQuery request, CancellationToken ct)
        {
            var texto = request.Texto ?? string.Empty;
            if (texto.Trim().Length < 2)
                return new List<NitDto>();

            var lista = await _repositorio.BuscarAsync(texto.Trim(), 10, ct);
            return lista.Select(n => new NitDto(n.Numero, n.Nombre)).ToList();
        }
    }
}
