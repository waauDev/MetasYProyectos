using MetasYProyectos.Domain.Entities;

namespace MetasYProyectos.Domain.Interfaces
{
    public interface INitRepository
    {
        Task<Nit?> ObtenerAsync(string numero, CancellationToken ct);

        Task<List<Nit>> BuscarAsync(string texto, int maximo, CancellationToken ct);
    }
}
