using ExpensesControl.WebApi.Config.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
            services.AddSwaggerGen(options =>
            {
                // Gera o documento para a versão v1
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpensesControlAPI", Version = "v1" });

                options.DocInclusionPredicate((version, apiDescription) =>
                {
                    var versions = apiDescription.ActionDescriptor.EndpointMetadata
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == version);
                });

                options.OperationFilter<SwaggerVersionOperationFilter>();
                options.EnableAnnotations();

                // Configuração de segurança
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });


            services.ConfigureOptions<SwaggerUIVersionConfig>();
        }

    }
}
