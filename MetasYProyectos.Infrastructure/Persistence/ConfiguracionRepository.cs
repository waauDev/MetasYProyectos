using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Enums;
using MetasYProyectos.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MetasYProyectos.Infrastructure.Persistence
{
    public sealed  class ConfiguracionRepository:IConfiguracionRepository
    {
        private readonly IDataProtector _protector;
        private readonly string _rutaArchivo;

        private const string Proposito = "MetasyProyectos.ConfiguracionBd.v1";
        public ConfiguracionRepository(
            IDataProtectionProvider dataProtectionProvider,
            IHostEnvironment env
            )
        {
            _protector = dataProtectionProvider.CreateProtector(Proposito);

            var carpetaConfig = Path.Combine(env.ContentRootPath, "Config");
            Directory.CreateDirectory(carpetaConfig);

            _rutaArchivo = Path.Combine(carpetaConfig, "bd.config");

        }

        public void Guardar(ConfiguracionBD configuracion)
        {
            var datos = new ConfiguracionPersistida
            {
                Servidor = configuracion.Servidor,
                Puerto = configuracion.Puerto,
                Servicio = configuracion.Servicio,
                Usuario = configuracion.Usuario,
                Password = configuracion.Password,
                Version = (int)configuracion.Version,
                TipoConexion = (int)configuracion.TipoConexion
            };

            var json = JsonSerializer.Serialize(datos);
            var encriptado = _protector.Protect(json);

            File.WriteAllText(_rutaArchivo, encriptado);
        }

        public ConfiguracionBD? Obtener()
        {
            if (!Existe())
                return null;
            try
            {
                var encriptado = File.ReadAllText(_rutaArchivo);
                var json = _protector.Unprotect(encriptado);
                var datos = JsonSerializer.Deserialize<ConfiguracionPersistida>(json);

                if (datos is null)
                    return null;

                return ConfiguracionBD.Crear(
                    datos.Servidor,
                    datos.Puerto,
                    datos.Servicio,
                    datos.Usuario,
                    datos.Password,
                    (VersionOracle)datos.Version,
                    (TipoConexion)datos.TipoConexion
                    );
                    
            }catch
            {
                return null;
            }
        }

        public bool Existe() => File.Exists(_rutaArchivo);

        private sealed class ConfiguracionPersistida
        {
            public string Servidor { get; set; } = string.Empty;
            public string Puerto { get; set; } = string.Empty;
            public string Servicio { get; set; } = string.Empty;
            public string Usuario { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public int Version { get; set; }
            public int TipoConexion { get; set; }
        }
    }
}
