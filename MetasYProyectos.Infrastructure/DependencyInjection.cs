using MetasYProyectos.Application.Interfaces;
using MetasYProyectos.Domain.Interfaces;
using MetasYProyectos.Infrastructure.Oracle;
using MetasYProyectos.Infrastructure.Persistence;
using MetasYProyectos.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Infrastructure
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(
            this IServiceCollection services)
        {
            

            services.AddSingleton<IConfiguracionRepository, ConfiguracionRepository>();
            services.AddScoped<IOracleConnectionChecker, OracleConnectionChecker>();

            services.AddScoped<IConfiguracionService, ConfiguracionService>();

            return services;
        }
    }
}
