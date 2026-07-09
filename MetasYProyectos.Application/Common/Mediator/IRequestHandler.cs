using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Common.Mediator
{
    public interface IRequestHandler<TRequest, TResponse> where TRequest:IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken ct);
    }
}
