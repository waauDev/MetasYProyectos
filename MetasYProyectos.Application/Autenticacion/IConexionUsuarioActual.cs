using System.Data;

namespace MetasYProyectos.Application.Autenticacion
{
    public interface IConexionUsuarioActual
    {
        string Usuario { get; }

        Task<IDbConnection> AbrirAsync(CancellationToken ct);
    }
}
