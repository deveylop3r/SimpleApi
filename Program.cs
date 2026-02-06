using Kompernass.Api.Core.Configuration;
using Kompernass.Api.Core.Extensions;
using SimpleApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

var kompernassOptions = new KompernassApiOptions
{
    ServiceName = "SimpleApi",
    Observability = new ObservabilityOptions
    {
        ServiceVersion = "1.0.0",
        MeterName = "SimpleApi.Metrics",
        ActivitySourceName = "SimpleApi.Operations",
        EnableConsoleExporters = true,
    },
    Security = new SecurityOptions
    {
        AddSecurityHeaders = true,
        EnableHsts = true,
        EnableRequestSizeLimits = true,
    },
    Api = new ApiOptions
    {
        EnableHealthChecks = true,
        EnableOpenApi = true,
        OpenApiContact = new OpenApiContactInfo
        {
            Name = "Leyla Calvimontes",
            Email = "calvimontes@kompernass.de"
        },
        EnableRequestLogging = true
    }
};

builder.AddKompernassApi(kompernassOptions);

// Add services to the container.
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddControllersWithExceptionHandling();

var app = builder.Build();

app.UseKompernassApi();

app.MapStandardApiEndpoints();

// Configure the HTTP request pipeline.
/* if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
} */

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
