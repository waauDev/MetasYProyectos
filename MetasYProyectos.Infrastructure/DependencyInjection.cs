using MetasYProyectos.Application.Autenticacion;
using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Domain.Interfaces;
using MetasYProyectos.Infrastructure.Autenticacion;
using MetasYProyectos.Infrastructure.Autenticacion.Pasos;
using MetasYProyectos.Infrastructure.Oracle;
using MetasYProyectos.Infrastructure.Persistence;
using MetasYProyectos.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MetasYProyectos.Infrastructure
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(
            this IServiceCollection services)
        {
            services.AddSingleton<IConfiguracionRepository, ConfiguracionRepository>();
            services.AddScoped<IPasoValidacionLogin, ValidarUsuarioExisteStep>();
            services.AddScoped<IPasoValidacionLogin, ValidarAplicativoStep>();
            services.AddScoped<IPasoValidacionLogin, ValidarUsuarioActivoStep>();
            services.AddScoped<IPasoValidacionLogin, ValidarPermisoStep>();

            services.AddScoped<IOracleConnectionChecker, OracleConnectionChecker>();
            services.AddScoped<IOracleConnectionFactory, OracleConnectionFactory>();

            services.AddScoped<IConfiguracionService, ConfiguracionService>();

            return services;
        }
    }
}
