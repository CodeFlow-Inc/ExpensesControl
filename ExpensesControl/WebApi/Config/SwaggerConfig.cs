using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace ExpensesControl.WebApi.Config
{
    /// <summary>
    /// Static class responsible for configuring Swagger for the ExpensesControlAPI.
    /// </summary>
    public static class SwaggerConfig
    {
        private static readonly string[] value = [];

        /// <summary>
        /// Configures Swagger with the specified settings, including security definitions and annotations.
        /// </summary>
        /// <param name="services">The service collection to which Swagger services will be added.</param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpensesControlAPI", Version = "v1" });

                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        value
                    }
                });
            });
        }
    }
}
