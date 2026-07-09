using MetasYProyectos.Domain.Autenticacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion
{
    public interface IPasoValidacionLogin
    {
        int Orden { get; }

        Task<ResultadoValidacion> ValidarAsync(
            IDbConnection conexion,
            CredencialesLogin credenciales,
            CancellationToken ct
            );
    }
}
