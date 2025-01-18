using Destructurama;
using ExpensesControl.WebApi.Config;
using ExpensesControl.WebApi.Config.Filters;
using ExpensesControl.WebApi.Config.Manager;
using ExpensesControl.WebApi.Extensions;
using Serilog;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// =====================================
// Logging Configuration with Serilog
// =====================================

await SerilogSeqDockerManager.ValidateDockerContainer();
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Destructure.UsingAttributes()
);
Log.Information("Starting up");

// =====================================
// Services Configuration
// =====================================

builder.Services.AddHealthChecks();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<AsyncExceptionFilter>();
    options.Filters.Add<RequestResponseLogFilter>();
})
.AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

// =====================================
// Middleware Pipeline Configuration
// =====================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestLocalization(CultureInfoExtensions.CreateRequestLocalizationOptions());

app.UseAuthorization();

app.MapControllers();

app.Run();
