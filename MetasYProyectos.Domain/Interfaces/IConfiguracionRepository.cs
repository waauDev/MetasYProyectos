using MetasYProyectos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Interfaces
{
    public interface IConfiguracionRepository
    {
        void Guardar(ConfiguracionBD configuracion);
        ConfiguracionBD? Obtener();
        bool Existe();
    }
}
