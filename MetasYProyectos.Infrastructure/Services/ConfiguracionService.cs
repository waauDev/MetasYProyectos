using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Application.Mappings;
using MetasYProyectos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Infrastructure.Services
{
    public sealed class ConfiguracionService:IConfiguracionService
    {
        private readonly IConfiguracionRepository _repositorio;
        private readonly IOracleConnectionChecker _connectionChecker;

        public ConfiguracionService(IConfiguracionRepository repositorio,
            IOracleConnectionChecker connectionChecker)
        {
            _repositorio = repositorio;
            _connectionChecker = connectionChecker;
        }

        public bool Existe() => _repositorio.Existe();
        

        public void Guardar(ConfiguracionBDDto dto)
        {
            var entidad = ConfiguracionMapper.ToEntity(dto);
            _repositorio.Guardar(entidad);
        }

        public ConfiguracionBDDto? Obtener()
        {
            var entidad = _repositorio.Obtener();
            return entidad is null
                ? null
                : ConfiguracionMapper.ToDto(entidad, ocultarPassword: true);
        }

        public async Task<(bool ok, string msg)> ProbarConexionAsync(ConfiguracionBDDto dto, CancellationToken ct = default)
        {
            var entidad = ConfiguracionMapper.ToEntity(dto);
            var (exitoso, mensaje) = await _connectionChecker.VerificarAsync(entidad, ct);
            return (exitoso, mensaje);
        }
    }
}
