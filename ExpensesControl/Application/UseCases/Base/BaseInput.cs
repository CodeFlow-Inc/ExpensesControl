using MediatR;

namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Base class for input in use cases.
/// Implements the <see cref="IRequest{TResponse}"/> interface from MediatR.
/// </summary>
/// <typeparam name="TOutput">The expected output type when processing the request.</typeparam>
public abstract class BaseInput<TOutput> : IRequest<TOutput>
{
}
