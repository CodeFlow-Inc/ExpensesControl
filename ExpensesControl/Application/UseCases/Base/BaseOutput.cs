namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Base class to represent the output of a use case.
/// </summary>
/// <typeparam name="T">The type of data contained in the output value.</typeparam>
public abstract class BaseOutput<T>
{
    /// <summary>
    /// The output value of the use case.
    /// </summary>
    public T? Value { get; private set; } = default;

    /// <summary>
    /// Unique flow identifier (FlowId) associated with the request.
    /// </summary>
    public Guid FlowId { get; private set; }

    /// <summary>
    /// List of error messages associated with processing the use case.
    /// </summary>
    private readonly List<string> _errorMessages = [];

    /// <summary>
    /// Read-only collection of error messages.
    /// </summary>
    public IReadOnlyCollection<string> ErrorMessages => _errorMessages.AsReadOnly();

    /// <summary>
    /// Indicates if the output is valid, meaning there are no error messages.
    /// </summary>
    public bool IsValid => _errorMessages.Count == 0;

    /// <summary>
    /// Sets the result of the use case.
    /// </summary>
    /// <param name="value">The value returned by the use case.</param>
    /// <param name="flowId">The unique flow identifier associated with the operation.</param>
    public void SetResult(T value, Guid flowId)
    {
        Value = value;
        FlowId = flowId;
    }

    /// <summary>
    /// Adds an error message to the list of error messages.
    /// </summary>
    /// <param name="errorMessage">The error message to be added.</param>
    public void AddErrorMessage(string errorMessage)
    {
        AddErrorMessages(errorMessage);
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A collection of error messages to be added.</param>
    public void AddErrorMessages(IEnumerable<string> errorMessages)
    {
        AddErrorMessages([.. errorMessages]);
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A list of error messages.</param>
    public void AddErrorMessages(params string[] errorMessages)
    {
        foreach (var errorMessage in errorMessages)
        {
            _errorMessages.Add(errorMessage);
        }
    }
}
