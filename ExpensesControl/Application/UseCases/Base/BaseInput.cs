using MediatR;

namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Base class for input in use cases.
/// Implements the <see cref="IRequest{TResponse}"/> interface from MediatR.
/// </summary>
/// <typeparam name="TOutput">The expected output type when processing the request.</typeparam>
public abstract class BaseInput<TOutput> : IRequest<TOutput>
{
    /// <summary>
    /// Unique identifier for the flow (FlowId).
    /// Used to trace requests in distributed scenarios and facilitate log correlation.
    /// </summary>
    public required Guid FlowId { get; set; }

    /// <summary>
    /// Sets the unique flow identifier (FlowId) for the request.
    /// This identifier is generally used for tracking in distributed scenarios.
    /// </summary>
    /// <param name="correlationId">The unique identifier for the request.</param>
    /// <returns>The same <see cref="BaseInput{TOutput}"/> object with the FlowId set.</returns>
    public BaseInput<TOutput> SetCorrelationId(Guid correlationId)
    {
        this.FlowId = correlationId;
        return this;
    }
}
