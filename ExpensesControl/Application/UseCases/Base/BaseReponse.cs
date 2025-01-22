namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Base class to represent the response of a use case.
/// </summary>
/// <typeparam name="T">The type of data contained in the response value.</typeparam>
public abstract class BaseResponse
{
    /// <summary>
    /// List of error messages associated with processing the use case.
    /// </summary>
    private readonly List<string> _errorMessages = [];

    /// <summary>
    /// Read-only collection of error messages.
    /// </summary>
    public IReadOnlyCollection<string> ErrorMessages => _errorMessages.AsReadOnly();

    /// <summary>
    /// Indicates if the response is valid, meaning there are no error messages.
    /// </summary>
    public bool IsValid => _errorMessages.Count == 0;


    /// <summary>
    /// Adds an error message to the list of error messages.
    /// </summary>
    /// <param name="errorMessage">The error message to be added.</param>
    public T AddErrorMessage<T>(string errorMessage) where T : BaseResponse
    {
        AddErrorMessages<T>(errorMessage);
        return (T)this;
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A collection of error messages to be added.</param>
    public T AddErrorMessages<T>(IEnumerable<string> errorMessages) where T : BaseResponse
    {
        AddErrorMessages<T>(errorMessages.ToArray());
        return (T)this;
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A list of error messages.</param>
    public T AddErrorMessages<T>(params string[] errorMessages) where T : BaseResponse
    {
        foreach (var errorMessage in errorMessages)
        {
            _errorMessages.Add(errorMessage);
        }
        return (T)this;
    }
}
