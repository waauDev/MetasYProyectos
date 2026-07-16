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

            var validatorTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

            foreach (var validator in validatorTypes)
            {
                var genericArg = validator.BaseType!.GetGenericArguments()[0];
                var serviceType = typeof(IValidator<>).MakeGenericType(genericArg);
                services.AddScoped(serviceType, validator);
            }

            return services;
                        
        }
    }
}
