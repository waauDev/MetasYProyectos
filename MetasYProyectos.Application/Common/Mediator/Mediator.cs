using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Common.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType)
                ?? throw new InvalidOperationException(
                  $"No se encontro un handler registrado para '{requestType.Name}' "
                    );
            var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))!;

            return (Task<TResponse>)method.Invoke(handler, new object[] { request, ct })!;
        }
    }
}
