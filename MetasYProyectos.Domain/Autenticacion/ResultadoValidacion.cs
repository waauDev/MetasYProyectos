using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Domain.Autenticacion
{
    public class ResultadoValidacion
    {
        public bool Existoso { get; set; }
        public string? MensajeError { get; init; }

        private ResultadoValidacion() { }

        public static ResultadoValidacion Ok() => new() { Existoso = true };
        public static ResultadoValidacion Fallo(string mensaje)
            => new() { Existoso = false, MensajeError= mensaje };
    }
}
