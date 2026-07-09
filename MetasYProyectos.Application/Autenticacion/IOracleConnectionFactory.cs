using MetasYProyectos.Domain.Autenticacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion
{
    
    public interface IOracleConnectionFactory
    {
        /// <summary>
        /// Abre una conexión Oracle autenticando con las credenciales del usuario final.
        /// Si el usuario/password son inválidos, Oracle rechaza la apertura (ORA-01017).
        /// Antes de devolver la conexión, ya deja fijado CURRENT_SCHEMA y NLS_DATE_FORMAT.
        /// </summary>
        /// 
        Task<IDbConnection> CrearConexionAsync(CredencialesLogin credenciales, CancellationToken ct);
    }
}
