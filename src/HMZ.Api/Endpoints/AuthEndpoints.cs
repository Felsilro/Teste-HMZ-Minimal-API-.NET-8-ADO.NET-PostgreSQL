using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using HMZ.Application.Handlers;
using HMZ.Application.Models;

namespace HMZ.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");
        group.MapPost("/login", async (AuthHandler handler, LoginRequest req) =>
        {
            var result = await handler.LoginAsync(req.Email, req.Password);
            return Results.Ok(result);
        }).WithName("Login");
        return app;
    }
}
