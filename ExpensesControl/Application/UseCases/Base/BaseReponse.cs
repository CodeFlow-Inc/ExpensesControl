using System.Text.Json.Serialization;

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
    public IReadOnlyCollection<string>? ErrorMessages => _errorMessages.Count == 0 ? null : _errorMessages.AsReadOnly();

    /// <summary>
    /// Indicates if the response is valid, meaning there are no error messages.
    /// </summary>
    public bool IsSuccess => _errorMessages.Count == 0;

    /// <summary>
    /// Gets or sets the trace identifier for the request, used for tracking and correlation purposes.
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Gets or sets the type of error, indicating whether it's a business rule violation or an internal system error.
    /// </summary>
    /// <value>
    /// The type of error. It can either be <see cref="ErrorType.BusinessRuleError"/> for business rule errors, or <see cref="ErrorType.InternalError"/> for internal system errors.
    /// </value>
    [JsonIgnore]
    public ErrorType? ErrorType { get; set; }
    /// <summary>
    /// Adds an error message to the list of error messages.
    /// </summary>
    /// <param name="errorMessage">The error message to be added.</param>
    public T AddErrorMessage<T>(string errorMessage, ErrorType errorType = (ErrorType)400) where T : BaseResponse
    {
        AddErrorMessages<T>(errorType, errorMessage);
        return (T)this;
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A collection of error messages to be added.</param>
    public T AddErrorMessages<T>(IEnumerable<string> errorMessages, ErrorType errorType = (ErrorType)400) where T : BaseResponse
    {
        AddErrorMessages<T>(errorType, errorMessages.ToArray());
        return (T)this;
    }

    /// <summary>
    /// Adds multiple error messages to the list of error messages.
    /// </summary>
    /// <param name="errorMessages">A list of error messages.</param>
    public T AddErrorMessages<T>(ErrorType errorType = (ErrorType)400, params string[] errorMessages) where T : BaseResponse
    {
        ErrorType = errorType;
        foreach (var errorMessage in errorMessages)
        {
            _errorMessages.Add(errorMessage);
        }
        return (T)this;
    }
}
