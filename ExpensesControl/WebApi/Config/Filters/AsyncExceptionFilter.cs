using ExpensesControl.Application.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ExpensesControl.WebApi.Config.Filters
{
    /// <summary>
    /// A global exception filter that handles all unhandled exceptions for the application.
    /// </summary>
    /// <remarks>
    /// This filter ensures consistent logging and error responses for both unhandled and service-specific exceptions.
    /// </remarks>
    /// <param name="logger">Logger instance for logging error details.</param>
    /// <param name="hostEnvironment">Host environment for determining the current environment context.</param>
    internal class AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger, IHostEnvironment hostEnvironment) : IAsyncExceptionFilter
    {
        /// <summary>
        /// Handles exceptions asynchronously, logs the error, and returns a standardized error response.
        /// </summary>
        /// <param name="context">The exception context containing details of the current HTTP request and exception.</param>
        /// <returns>A completed task once the exception handling is done.</returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var referenceId = Guid.NewGuid().ToString();

            logger.LogError(exception, "UnhandledException: {ExceptionType} - {Message}. {Log}", exception.GetType(), exception.Message, $"ReferenceId: {referenceId}");

            var message = "Ocorreu um erro inesperado. Verifique os dados ou tente novamente mais tarde.";
            var forDevs = $"Você pode usar o seguinte ID de referência para nos ajudar a diagnosticar seu problema: {referenceId}";

            var responseContent = new
            {
                message,
                forDevs,
                referenceId
            };

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(responseContent),
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "application/json"
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Ensures that a given string ends with a period.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <returns>The string with a period appended if not already present.</returns>
        private static string EnsureStringEndsInPeriod(string s)
        {
            return $"{s.TrimEnd('.')}."; // Ensure proper punctuation
        }

        /// <summary>
        /// Escapes braces in a string for safe use in string formatting.
        /// </summary>
        /// <param name="s">The input string to escape.</param>
        /// <returns>The escaped string with braces doubled.</returns>
        private static string EscapeJsonForStringFormatInput(string s)
        {
            return s.Replace("{", "{{").Replace("}", "}}");
        }

        /// <summary>
        /// Generates a standardized result for generic exceptions.
        /// </summary>
        /// <param name="exception">The exception to process.</param>
        /// <returns>An <see cref="IActionResult"/> with a standardized error message.</returns>
        private ContentResult GetResult(Exception exception)
        {
            var exceptionDescription = exception.GetType().ToString();

            if (!string.IsNullOrEmpty(exception.Message))
            {
                exceptionDescription += $" - {exception.Message}";
            }

            exceptionDescription = EnsureStringEndsInPeriod(exceptionDescription);
            exceptionDescription = EscapeJsonForStringFormatInput(exceptionDescription);

            var log = new ErrorLog
            {
                ErrorLogId = Guid.NewGuid().ToString()
            };

            logger.LogError(exception, "UnhandledException: {ExceptionDescription} {{{Log}}}", exceptionDescription, log);

            var content = $"An unexpected error has occurred. You can use the following reference ID to help us diagnose your problem: {log.ErrorLogId}";

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(content),
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// Generates a standardized result for service-specific exceptions.
        /// </summary>
        /// <param name="exception">The service exception to process.</param>
        /// <returns>An <see cref="IActionResult"/> with detailed error information.</returns>
        private ObjectResult GetResult(ServiceException exception)
        {
            var responseBody = new ProblemDetails
            {
                Type = exception.ErrorCode.ToString(),
                Title = exception.ErrorCode.GetDescription(),
                Detail = exception.Detail
            };

            if (hostEnvironment.IsDevelopment())
            {
                responseBody.Extensions.Add("stackTrace", exception.ToString());
            }

            var referenceId = Guid.NewGuid().ToString();
            var log = new ServiceExceptionLog
            {
                ServiceExceptionLogId = referenceId
            };

            var exceptionDescription = exception.Message;
            exceptionDescription = EnsureStringEndsInPeriod(exceptionDescription);
            exceptionDescription = EscapeJsonForStringFormatInput(exceptionDescription);

            logger.LogInformation(exception, "ServiceException: {ExceptionDescription} {{{Log}}}", exceptionDescription, log);

            responseBody.Status = exception.ErrorCode switch
            {
                ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
                ErrorCode.InvalidRequest => StatusCodes.Status400BadRequest,
                ErrorCode.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            var result = new ObjectResult(responseBody)
            {
                StatusCode = responseBody.Status,
                ContentTypes = { "application/problem+json", "application/problem+xml" }
            };

            return result;
        }

        /// <summary>
        /// Represents additional details for a generic error log.
        /// </summary>
        private sealed class ErrorLog
        {
            /// <summary>
            /// The unique identifier for the error log.
            /// </summary>
            public string ErrorLogId { get; init; } = default!;

            /// <summary>
            /// The unique identifier for the audit log.
            /// </summary>
            public string AuditLogId { get; set; } = default!;

            /// <summary>
            /// The unique identifier for the request log.
            /// </summary>
            public string RequestId { get; set; } = default!;

            /// <summary>
            /// Returns a string representation of the error log, including the reference ID.
            /// </summary>
            /// <returns>A string containing the reference ID.</returns>
            public override string ToString()
            {
                return $"ReferenceId: {ErrorLogId}";
            }
        }

        /// <summary>
        /// Represents additional details for a service exception log.
        /// </summary>
        private sealed class ServiceExceptionLog
        {
            /// <summary>
            /// The unique identifier for the service exception log.
            /// </summary>
            public string ServiceExceptionLogId { get; init; } = default!;

            /// <summary>
            /// The unique identifier for the audit log.
            /// </summary>
            public string AuditLogId { get; set; } = default!;

            /// <summary>
            /// Returns a string representation of the service exception log, including the reference ID.
            /// </summary>
            /// <returns>A string containing the reference ID.</returns>
            public override string ToString()
            {
                return $"ReferenceId: {ServiceExceptionLogId}";
            }
        }
    }
}
