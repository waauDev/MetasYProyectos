using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion.Excepciones
{
    public class ConexionBaseDatosException:Exception
    {
        public ConexionBaseDatosException(string mensaje, Exception inner)
            : base(mensaje, inner) 
        {
            
        }
    }
}
