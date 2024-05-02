

namespace CBT3_Application.Interfaces;

//public interface IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
//{
//    Task<TResponse> HandleAsync(TRequest request);
//}

public interface IRequestHandler<in TRequest> where TRequest : class, IRequest
{
    Task HandleAsync(TRequest request, CancellationToken cancellation = default);
}
public interface IRequestHandler<in TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellation = default);
}

