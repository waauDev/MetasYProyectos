using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Autenticacion
{
    public record CredencialesLogin(string Usuario, string password, string BaseDatos, string vigencia);


}
