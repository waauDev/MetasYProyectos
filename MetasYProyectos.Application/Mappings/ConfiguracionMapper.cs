using MetasYProyectos.Application.DTOs;
using MetasYProyectos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Mappings
{
    public static class ConfiguracionMapper
    {
        public static ConfiguracionBD ToEntity(ConfiguracionBDDto dto)
            => ConfiguracionBD.Crear(
                    dto.Nombre,
                    dto.Servidor,
                    dto.Puerto,
                    dto.Servicio,
                    dto.Usuario,
                    dto.Password,
                    dto.Version,
                    dto.TipoConexion
                );


        public static ConfiguracionBDDto ToDto(
            ConfiguracionBD entidad,
            bool ocultarPassword = false)
            => new()
            {
                Nombre = entidad.Nombre,
                Servidor = entidad.Servidor,
                Puerto = entidad.Puerto,
                Servicio = entidad.Servicio,
                Usuario = entidad.Usuario,
                Password = ocultarPassword ? "••••••••" : entidad.Password,
                Version = entidad.Version,
                TipoConexion = entidad.TipoConexion
            };


    }
}
