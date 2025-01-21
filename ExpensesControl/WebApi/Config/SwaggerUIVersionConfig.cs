using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExpensesControl.WebApi.Config
{
    public class SwaggerUIVersionConfig : IConfigureOptions<SwaggerUIOptions>
    {
        public void Configure(SwaggerUIOptions options)
        {
            // Configure dynamic version handling in Swagger UI
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpensesControl API v1");

            // Additional Swagger UI settings
            options.DefaultModelsExpandDepth(-1); // Hides schema by default
            options.DisplayRequestDuration();
            options.EnableDeepLinking();

            options.RoutePrefix = "swagger";
        }
    }
}
