using Destructurama;
using ExpensesControl.Domain.Enums;
using ExpensesControl.Infrastructure.SqlServer.Ioc;
using ExpensesControl.WebApi.Config;
using ExpensesControl.WebApi.Config.Filters;
using ExpensesControl.WebApi.Extensions;
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
builder.Services.ConfigureRepositoryIoc();

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
    o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<ExpenseCategory>());
    o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<RecurrencePeriodicity>());
    o.JsonSerializerOptions.Converters.Add(new DescriptionEnumConverter<PaymentType>());
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
