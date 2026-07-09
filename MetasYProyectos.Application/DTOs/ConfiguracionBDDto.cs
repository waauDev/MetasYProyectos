
using MetasYProyectos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.DTOs
{
    public sealed record ConfiguracionBDDto
    {
        public string Servidor  { get; set; } = string.Empty;
        public string Puerto    { get; set; } = "1521";
        public string Servicio  { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public VersionOracle Version { get; set; } = VersionOracle.Oracle11;
        public TipoConexion TipoConexion { get; set; } = TipoConexion.ServiceName;

    }
}
