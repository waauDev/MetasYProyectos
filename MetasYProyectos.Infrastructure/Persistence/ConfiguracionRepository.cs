using MetasYProyectos.Domain.Entities;
using MetasYProyectos.Domain.Enums;
using MetasYProyectos.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace MetasYProyectos.Infrastructure.Persistence
{
    public sealed class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly IDataProtector _protector;
        private readonly string _rutaArchivo;

        private const string Proposito = "MetasyProyectos.ConfiguracionBd.v1";

        public ConfiguracionRepository(
            IDataProtectionProvider dataProtectionProvider,
            IHostEnvironment env)
        {
            _protector = dataProtectionProvider.CreateProtector(Proposito);

            var carpetaConfig = Path.Combine(env.ContentRootPath, "Config");
            Directory.CreateDirectory(carpetaConfig);

            _rutaArchivo = Path.Combine(carpetaConfig, "bd.config");
        }

        public void Guardar(ConfiguracionBD configuracion)
        {
            var lista = CargarTodas();

            var existente = lista.FindIndex(c =>
                string.Equals(c.Nombre, configuracion.Nombre, StringComparison.OrdinalIgnoreCase));

            var datos = new ConfiguracionPersistida
            {
                Nombre = configuracion.Nombre,
                Servidor = configuracion.Servidor,
                Puerto = configuracion.Puerto,
                Servicio = configuracion.Servicio,
                Usuario = configuracion.Usuario,
                Password = configuracion.Password,
                Version = (int)configuracion.Version,
                TipoConexion = (int)configuracion.TipoConexion
            };

            if (existente >= 0)
                lista[existente] = datos;
            else
                lista.Add(datos);

            GuardarLista(lista);
        }

        public ConfiguracionBD? ObtenerPorNombre(string nombre)
        {
            var lista = CargarTodas();
            var datos = lista.Find(c =>
                string.Equals(c.Nombre, nombre, StringComparison.OrdinalIgnoreCase));

            return datos is null ? null : AEntidad(datos);
        }

        public List<ConfiguracionBD> ObtenerTodas()
        {
            return CargarTodas().Select(AEntidad).ToList();
        }

        public void Eliminar(string nombre)
        {
            var lista = CargarTodas();
            lista.RemoveAll(c =>
                string.Equals(c.Nombre, nombre, StringComparison.OrdinalIgnoreCase));
            GuardarLista(lista);
        }

        public bool ExisteAlguna() => CargarTodas().Count > 0;

        private List<ConfiguracionPersistida> CargarTodas()
        {
            if (!File.Exists(_rutaArchivo))
                return new List<ConfiguracionPersistida>();

            try
            {
                var encriptado = File.ReadAllText(_rutaArchivo);
                var json = _protector.Unprotect(encriptado);
                return JsonSerializer.Deserialize<List<ConfiguracionPersistida>>(json)
                       ?? new List<ConfiguracionPersistida>();
            }
            catch
            {
                return new List<ConfiguracionPersistida>();
            }
        }

        private void GuardarLista(List<ConfiguracionPersistida> lista)
        {
            var json = JsonSerializer.Serialize(lista);
            var encriptado = _protector.Protect(json);
            File.WriteAllText(_rutaArchivo, encriptado);
        }

        private static ConfiguracionBD AEntidad(ConfiguracionPersistida datos)
            => ConfiguracionBD.Crear(
                datos.Nombre,
                datos.Servidor,
                datos.Puerto,
                datos.Servicio,
                datos.Usuario,
                datos.Password,
                (VersionOracle)datos.Version,
                (TipoConexion)datos.TipoConexion);

        private sealed class ConfiguracionPersistida
        {
            public string Nombre { get; set; } = string.Empty;
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
