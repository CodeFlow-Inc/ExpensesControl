using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpensesControl.WebApi.Config.Filters
{
    /// <summary>
    /// Filter to log HTTP request and response information.
    /// </summary>
    /// <param name="logger">Logger instance for writing log messages.</param>
    /// <param name="config">Configuration instance to retrieve application settings.</param>
    public class RequestResponseLogFilter(ILogger<RequestResponseLogFilter> logger, IConfiguration config) : IAsyncActionFilter
    {
        /// <summary>
        /// Indicates whether request and response logging is enabled.
        /// </summary>
        private readonly bool _isRequestResponseLoggingEnabled = config.GetValue("EnableRequestResponseLogging", false);

        /// <summary>
        /// Formats HTTP headers into a readable string.
        /// </summary>
        /// <param name="headers">The headers to format.</param>
        /// <returns>A formatted string representation of the headers.</returns>
        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value!)}}}"));

        /// <summary>
        /// Asynchronously logs HTTP request and response information if logging is enabled.
        /// </summary>
        /// <param name="context">The context for the executing action.</param>
        /// <param name="next">Delegate to execute the next action filter or action.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Filter is enabled only when the EnableRequestResponseLogging config value is set.
            if (_isRequestResponseLoggingEnabled)
            {
                var requestBody = context.HttpContext.Request.Method == "GET" ? "" : context.ActionArguments.First().Value;

                logger.LogDebug("HTTP request information:\n" +
                        "\tMethod: {@method}\n" +
                        "\tPath: {@path}\n" +
                        "\tQueryString: {@queryString}\n" +
                        "\tHeaders: {@headers}\n" +
                        "\tSchema: {@scheme}\n" +
                        "\tHost: {@host}\n" +
                        "\tRequest Body: {@requestBody}",
                        context.HttpContext.Request.Method,
                        context.HttpContext.Request.Path,
                        context.HttpContext.Request.QueryString,
                        FormatHeaders(context.HttpContext.Request.Headers),
                        context.HttpContext.Request.Scheme,
                        context.HttpContext.Request.Host,
                        requestBody);

                var responseContext = await next();

                var result = responseContext.Result as Microsoft.AspNetCore.Mvc.ObjectResult;
                logger.LogDebug("HTTP response information:\n" +
                        "\tStatusCode: {@statusCode}\n" +
                        "\tContentType: {@contentType}\n" +
                        "\tHeaders: {@headers}\n" +
                        "\tBody: {@response}",
                        responseContext.HttpContext.Response.StatusCode,
                        responseContext.HttpContext.Response.ContentType,
                        FormatHeaders(responseContext.HttpContext.Response.Headers),
                        result?.Value);
            }
            else
            {
                await next();
            }
        }
    }
}
