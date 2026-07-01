using MetasYProyectos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Interfaces
{
   public interface IConfiguracionService
    {
        void Guardar(ConfiguracionBDDto dto);
        ConfiguracionBDDto? Obtener();
        bool Existe();
        Task<(bool ok, string msg)> ProbarConexionAsync(
            ConfiguracionBDDto dto,
            CancellationToken ct = default
            );
    }
}
