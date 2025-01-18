using MediatR;

namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Classe base para entradas de uso de casos (Use Cases).
/// Implementa a interface <see cref="IRequest{TResponse}"/> do MediatR.
/// </summary>
/// <typeparam name="TOutput">O tipo de saída esperado ao processar a requisição.</typeparam>
public abstract class BaseInput<TOutput> : IRequest<TOutput>
{
    /// <summary>
    /// Identificador único do fluxo (FlowId).
    /// Utilizado para rastrear requisições em cenários distribuídos e facilitar a correlação de logs.
    /// </summary>
    public required Guid FlowId { get; set; }

    /// <summary>
    /// Define o identificador único do fluxo (FlowId) para a requisição.
    /// Este identificador é geralmente utilizado para rastreamento em cenários distribuídos.
    /// </summary>
    /// <param name="correlationId">O identificador único da requisição.</param>
    /// <returns>O próprio objeto <see cref="BaseInput{TOutput}"/> com o FlowId definido.</returns>
    public BaseInput<TOutput> SetCorrelationId(Guid correlationId)
    {
        this.FlowId = correlationId;
        return this;
    }
}
