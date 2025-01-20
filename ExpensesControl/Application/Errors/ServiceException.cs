namespace ExpensesControl.Application.Errors
{
    /// <summary>
    /// A service exception.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ServiceException"/> class.
    /// </remarks>
    /// <param name="errorCode"></param>
    /// <param name="innerException"></param>
    /// <param name="detail"></param>
    public class ServiceException(ErrorCode errorCode, Exception? innerException, string? detail = null) : Exception(GetMessage(errorCode, innerException, detail), innerException)
    {
        /// <summary>
        /// Gets the canonical error code that represents the business error.
        /// </summary>
        public ErrorCode ErrorCode { get; } = errorCode;

        /// <summary>
        /// Gets the detail for the given service exception.
        /// </summary>
        public string? Detail { get; } = detail;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceException"/> class.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="detail"></param>
        public ServiceException(ErrorCode errorCode, string? detail = null)
            : this(errorCode, null, detail)
        {
        }

        private static string GetMessage(ErrorCode errorCode, Exception? innerException, string? detail)
        {
            var message = $"Code: {errorCode}";

            if (innerException != null)
            {
                var innerExceptionMessage = string.Empty;
                if (!string.IsNullOrEmpty(innerException.Message))
                {
                    innerExceptionMessage = $" - {innerException.Message}";
                }

                message += $"\nInner Exception: {innerException.GetType()}{innerExceptionMessage}";
            }

            if (detail != null)
            {
                message += $"\nDetail: {detail}";
            }

            return message;
        }
    }
}
