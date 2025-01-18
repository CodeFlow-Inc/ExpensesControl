using System.Text.Json.Serialization;
using Serilog;
using Destructurama;
using ExpensesControl.WebApi.Config;
using ExpensesControl.WebApi.Config.Filters;
using ExpensesControl.WebApi.Config.Manager;
using ExpensesControl.WebApi.Extensions;
var builder = WebApplication.CreateBuilder(args);

// =====================================
// Configuração de Logs com Serilog
// =====================================

await SerilogSeqDockerManager.ValidateDockerContainer();
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Destructure.UsingAttributes()
);
Log.Information("Starting up");

// =====================================
// Configuração de Serviços
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
// Configuração do Pipeline de Middleware
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
