using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using HMZ.Application.Handlers;
using HMZ.Application.Models;

namespace HMZ.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").RequireAuthorization();

        group.MapGet("/", async (UserHandler handler, int page = 1, int per_page = 5) =>
        {
            var result = await handler.GetPagedAsync(page, per_page);
            return Results.Ok(result);
        }).WithName("ListUsers");

        group.MapGet("/{id:int}", async (UserHandler handler, int id) =>
        {
            var user = await handler.GetByIdAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        }).WithName("GetUserById");

        group.MapPut("/{id:int}", async (UserHandler handler, int id, UpdateUserRequest req) =>
        {
            var updated = await handler.UpdateAsync(id, req);
            return updated ? Results.NoContent() : Results.NotFound();
        }).WithName("UpdateUserById");

        group.MapDelete("/{id:int}", async (UserHandler handler, int id) =>
        {
            var deleted = await handler.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteUserById");

        return app;
    }
}
