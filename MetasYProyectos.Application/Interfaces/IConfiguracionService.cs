using MetasYProyectos.Application.DTOs;

namespace MetasYProyectos.Application.Interfaces
{
    public interface IConfiguracionService
    {
        void Guardar(ConfiguracionBDDto dto);
        ConfiguracionBDDto? ObtenerPorNombre(string nombre);
        List<ConfiguracionBDDto> ObtenerTodas();
        void Eliminar(string nombre);
        bool ExisteAlguna();
        Task<(bool ok, string msg)> ProbarConexionAsync(
            ConfiguracionBDDto dto,
            CancellationToken ct = default
            );
    }
}
