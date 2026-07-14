using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Interfaces;

namespace MetasYProyectos.Infrastructure.Services
{
    public sealed class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repositorio;
        private readonly IOracleConnectionChecker _connectionChecker;

        public ConfiguracionService(IConfiguracionRepository repositorio,
            IOracleConnectionChecker connectionChecker)
        {
            _repositorio = repositorio;
            _connectionChecker = connectionChecker;
        }

        public bool ExisteAlguna() => _repositorio.ExisteAlguna();

        public void Guardar(ConfiguracionBDDto dto)
        {
            var entidad = ConfiguracionMapper.ToEntity(dto);
            _repositorio.Guardar(entidad);
        }

        public ConfiguracionBDDto? ObtenerPorNombre(string nombre)
        {
            var entidad = _repositorio.ObtenerPorNombre(nombre);
            return entidad is null
                ? null
                : ConfiguracionMapper.ToDto(entidad, ocultarPassword: true);
        }

        public List<ConfiguracionBDDto> ObtenerTodas()
        {
            return _repositorio.ObtenerTodas()
                .Select(e => ConfiguracionMapper.ToDto(e, ocultarPassword: true))
                .ToList();
        }

        public void Eliminar(string nombre) => _repositorio.Eliminar(nombre);

        public async Task<(bool ok, string msg)> ProbarConexionAsync(ConfiguracionBDDto dto, CancellationToken ct = default)
        {
            var entidad = ConfiguracionMapper.ToEntity(dto);
            var (exitoso, mensaje) = await _connectionChecker.VerificarAsync(entidad, ct);
            return (exitoso, mensaje);
        }
    }
}
