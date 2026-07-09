using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Autenticacion.Excepciones
{
    public class CredencialesInvalidasException:Exception
    {
        public CredencialesInvalidasException()
            :base("Usuario o contraseña incorrectos")
        {
            
        }
    }
}
