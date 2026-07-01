using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Exceptions
{
    public class ConfiguracionInvalidaException:Exception
    {
        public string Campo { get; }

        public ConfiguracionInvalidaException(string campo, string mensaje):base(mensaje) 
        {
            Campo= campo;
        }

        public ConfiguracionInvalidaException(string mensaje) : base(mensaje)
        {
            Campo = string.Empty;
        }
    }
}
