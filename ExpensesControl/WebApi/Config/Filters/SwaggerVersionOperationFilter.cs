using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExpensesControl.WebApi.Config.Filters
{
    public class SwaggerVersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Ensure that the parameter is not added if it is already present in the route
            if (context.ApiDescription.RelativePath?.Contains("{version}") == true)
            {
                return;
            }

            operation.Parameters ??= [];
        }
    }
}
