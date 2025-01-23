using MediatR;

namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Base class for request in use cases.
/// Implements the <see cref="IRequest{TResponse}"/> interface from MediatR.
/// </summary>
/// <typeparam name="TResponse">The expected response type when processing the request.</typeparam>
public abstract class BaseRequest<TResponse> : IRequest<TResponse>
{
}
