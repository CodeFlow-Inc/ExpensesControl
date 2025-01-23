namespace ExpensesControl.Application.UseCases.Base;

/// <summary>
/// Enum to categorize error types.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Represents an internal system error.
    /// </summary>
    InternalError = 500,

    /// <summary>
    /// Represents a business rule error.
    /// </summary>
    BusinessRuleError = 400
}
