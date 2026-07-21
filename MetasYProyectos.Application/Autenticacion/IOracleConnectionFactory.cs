using MetasYProyectos.Domain.Autenticacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion
{
    
    public interface IOracleConnectionFactory
    {
        Task<IDbConnection> CrearConexionTecnicaAsync(string baseDatos, CancellationToken ct);

        Task<IDbConnection> CrearConexionUsuarioAsync(CredencialesLogin credenciales, CancellationToken ct);
    }
}
