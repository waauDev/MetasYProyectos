using MetasYProyectos.Domain.Entities;

namespace MetasYProyectos.Domain.Interfaces
{
    public interface IConfiguracionRepository
    {
        void Guardar(ConfiguracionBD configuracion);
        ConfiguracionBD? ObtenerPorNombre(string nombre);
        List<ConfiguracionBD> ObtenerTodas();
        void Eliminar(string nombre);
        bool ExisteAlguna();
    }
}
