using System.ComponentModel;

namespace ExpensesControl.Application.Errors
{
    /// <summary>
    /// Enum for error codes for logging purpose.
    /// </summary>
    public enum ErrorCode
    {
        [Description("The request failed due to server error.")]
        InternalServerError,

        [Description("The user is not authorised to perform the operation.")]
        Forbidden,

        [Description("Resource not found.")]
        NotFound,

        [Description("Request Invalid.")]
        InvalidRequest
    }
}
