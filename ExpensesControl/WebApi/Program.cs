using Destructurama;
using ExpensesControl.Domain.Enums;
using ExpensesControl.WebApi.Config;
using ExpensesControl.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json.Serialization;
using static ExpensesControl.Application.Extensions.EnumExtensions;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// Logging Configuration with Serilog
// =====================================

builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Destructure.UsingAttributes(x => x.IgnoreNullProperties = true)
);
Log.Information("Starting up");

// =====================================
// Services Configuration
// =====================================

string? sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.ConfigureDatabaseSqlServer(sqlConnection!);
builder.Services.UpdateMigrationDatabase();

builder.Services.AddDependencyInjection();

builder.Services.AddHealthChecks();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services
    .AddControllers()
        .AddJsonOptions(static o =>
        {
            o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<ExpenseCategory>());
            o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<RecurrencePeriodicity>());
            o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<PaymentType>());
        });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.ConfigureSwagger();

// =====================================
// Middleware Pipeline Configuration
// =====================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpensesControlAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseRequestLocalization(CultureInfoExtensions.CreateRequestLocalizationOptions());

app.UseAuthorization();

app.MapControllers();

app.Run();
