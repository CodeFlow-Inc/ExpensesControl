using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExpensesControl.WebApi.Config.Filters
{
    /// <summary>
    /// A filter to handle the Swagger documentation for versioning.
    /// Ensures the version parameter is added only when it is not already part of the route.
    /// </summary>
    public class SwaggerVersionOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to modify the OpenAPI operation metadata.
        /// </summary>
        /// <param name="operation">The OpenAPI operation being processed.</param>
        /// <param name="context">The context containing information about the API operation.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Ensure that the parameter is not added if it is already present in the route
            if (context.ApiDescription.RelativePath?.Contains("{version}") == true)
            {
                return;
            }

            // Initialize the operation parameters if null
            operation.Parameters ??= new List<OpenApiParameter>();
        }
    }
}
