using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HMZ.Application.Configuration;

public static class CorsSetup
{
    public const string MyAllowSpecificOrigins = "_hmzCors";
}

public static class DependencyInjection
{
    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsSetup.MyAllowSpecificOrigins, policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "hmz-issuer";
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "hmz-audience";
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "super-secret-change-me-please-32chars";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddDatabaseContexts(this IServiceCollection services)
    {
        services.AddSingleton<HMZ.Domain.Database.IConnectionFactory, HMZ.Domain.Database.NpgsqlConnectionFactory>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<HMZ.Domain.Repositories.IUserRepository, HMZ.Domain.Repositories.AdoUserRepository>();
        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<HMZ.Application.Handlers.AuthHandler>();
        services.AddSingleton<HMZ.Application.Handlers.UserHandler>();
        services.AddSingleton<HMZ.Application.Services.JwtTokenService>();
        services.AddSingleton<HMZ.Application.Services.PasswordHasher>();
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HMZ Minimal API", Version = "v1" });
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Description = "Paste your JWT Bearer token here"
            };
            c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });
        return services;
    }
}
