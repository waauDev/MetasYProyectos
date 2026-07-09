using System;
using System.Collections.Generic;
using System.Text;

namespace MetasYProyectos.Application.Common.Mediator
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
    }
}
