namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Classe base para representar a saída de um caso de uso (Use Case).
/// </summary>
/// <typeparam name="T">O tipo de dado contido no valor de saída.</typeparam>
public abstract class BaseOutput<T>
{
    /// <summary>
    /// O valor da saída do caso de uso.
    /// </summary>
    public T? Value { get; private set; } = default;

    /// <summary>
    /// Identificador único do fluxo (FlowId) associado à requisição.
    /// </summary>
    public Guid FlowId { get; private set; }

    /// <summary>
    /// Lista de mensagens de erro associadas ao processamento do caso de uso.
    /// </summary>
    private readonly List<string> _errorMessages = [];

    /// <summary>
    /// Coleção somente leitura de mensagens de erro.
    /// </summary>
    public IReadOnlyCollection<string> ErrorMessages => _errorMessages.AsReadOnly();

    /// <summary>
    /// Indica se a saída é válida, ou seja, se não há mensagens de erro.
    /// </summary>
    public bool IsValid => _errorMessages.Count == 0;

    /// <summary>
    /// Define o resultado do caso de uso.
    /// </summary>
    /// <param name="value">O valor retornado pelo caso de uso.</param>
    /// <param name="flowId">O identificador único do fluxo associado à operação.</param>
    public void SetResult(T value, Guid flowId)
    {
        Value = value;
        FlowId = flowId;
    }

    /// <summary>
    /// Adiciona uma mensagem de erro à lista de mensagens de erro.
    /// </summary>
    /// <param name="errorMessage">A mensagem de erro a ser adicionada.</param>
    public void AddErrorMessage(string errorMessage)
    {
        AddErrorMessages(errorMessage);
    }

    /// <summary>
    /// Adiciona várias mensagens de erro à lista de mensagens de erro.
    /// </summary>
    /// <param name="errorMessages">Uma coleção de mensagens de erro a serem adicionadas.</param>
    public void AddErrorMessages(IEnumerable<string> errorMessages)
    {
        AddErrorMessages([.. errorMessages]);
    }

    /// <summary>
    /// Adiciona múltiplas mensagens de erro à lista de mensagens de erro.
    /// </summary>
    /// <param name="errorMessages">Uma lista de mensagens de erro.</param>
    public void AddErrorMessages(params string[] errorMessages)
    {
        foreach (var errorMessage in errorMessages)
        {
            _errorMessages.Add(errorMessage);
        }
    }
}
