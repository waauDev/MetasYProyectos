namespace MetasYProyectos.Application.Interfaces
{
    public interface IValidadorPermisoAccion
    {
        Task<bool> TienePermisoAsync(string codAccion, CancellationToken ct);
    }
}
