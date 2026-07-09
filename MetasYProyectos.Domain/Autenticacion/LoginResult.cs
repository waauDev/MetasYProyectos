using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Autenticacion
{
    public class LoginResult
    {
        public bool Existoso { get; init; }
        public string? MensajeError { get; init; }
        public CredencialesLogin? Credenciales { get; init; }

        private LoginResult() { }

        public static LoginResult Ok(CredencialesLogin credenciales)
            => new() { Existoso = true, Credenciales = credenciales };

        public static LoginResult Fallo(string mensaje)
            => new() { Existoso=false, MensajeError= mensaje };



    }
}
