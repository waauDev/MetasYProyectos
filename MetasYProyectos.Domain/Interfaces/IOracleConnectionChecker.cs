using MetasYProyectos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Interfaces
{
    public interface IOracleConnectionChecker
    {
        Task<(bool exitoso, string mensaje)> VerificarAsync(ConfiguracionBD configuracion, CancellationToken cancellationToken = default);
    }
}
