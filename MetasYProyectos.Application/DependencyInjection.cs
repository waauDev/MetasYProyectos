using FluentValidation;
using MetasYProyectos.Application.Common.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MetasYProyectos.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            var assembly = Assembly.GetExecutingAssembly();

            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .Select(i => new { Interface = i, Implementation = t }));
            
            foreach  (var handler in handlerTypes){
                services.AddScoped(handler.Interface, handler.Implementation);
            }

            return services;
                        
        }
    }
}
