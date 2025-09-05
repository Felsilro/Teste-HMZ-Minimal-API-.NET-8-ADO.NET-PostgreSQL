using HMZ.Api.Endpoints;
using HMZ.Application.Configuration;
using DotNetEnv;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

if (!string.Equals(Environment.GetEnvironmentVariable("ENVIRONMENT"), "PRD", StringComparison.OrdinalIgnoreCase))
    Env.Load("../../.env");

builder.Services
    .AddHttpClient()
    .AddHttpContextAccessor()
    .AddCorsPolicies()
    .AddPresentation()
    .AddJwtAuthentication()
    .AddAuthorizationPolicies()
    .AddDatabaseContexts()
    .AddRepositories()
    .AddHandlers();

builder.Services.ConfigureSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

var enableSwagger = Environment.GetEnvironmentVariable("SWAGGER_DOCS") == "1" || app.Environment.IsDevelopment();
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

// Em produção, mantenha redirect HTTPS; em dev Linux não é necessário
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors(CorsSetup.MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();

app.AddAuthEndpoints();
app.AddUserEndpoints();

Serilog.Log.Warning("########## HMZ API {Version} ##########", "v1.3.1");
app.Run();

public partial class Program { }
