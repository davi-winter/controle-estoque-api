using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.UseCases.Products;
using InventoryControl.Application.UseCases.Users;
using System.Security.Claims;

namespace InventoryControl.API.Endpoints
{
    public static class UsersEndpoints
    {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users")
                           .WithTags("Users");

            // POST /api/users/login
            group.MapPost("/login", async (LoginRequest request, LoginUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response?.IsFailure == true)
                {
                    switch (response.Error?.Code)
                    {
                        case "User.NotFound":
                            return Results.NotFound(response.Error);
                        case "User.InvalidCredentials":
                            return Results.Unauthorized();
                        default:
                            return Results.BadRequest(response.Error);
                    }
                }

                return Results.Ok(response?.Value);
            })
            .WithName("Login")
            .Produces<LoginResponse>(StatusCodes.Status200OK);

            // POST /api/users
            group.MapPost("/", async (CreateUserRequest request, CreateUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response?.IsFailure == true)
                    return Results.BadRequest(response.Error);

                return Results.Created($"/api/users/{response?.Value?.Id}", response?.Value);
            })
            .WithName("CreateUser")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .RequireAuthorization();

            // PUT /api/users/{id}
            group.MapPut("/{id}", async (Guid id, CreateUserRequest request, UpdateUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                if (response?.IsFailure == true)
                    return Results.BadRequest(response.Error);

                return Results.Ok(response?.Value);
            })
            .WithName("UpdateUser")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();

            // DELETE /api/users/{id}
            group.MapDelete("/{id}", async (Guid id, DeleteUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id);

                if (response?.IsFailure == true)
                    return Results.BadRequest(response.Error);

                return Results.NoContent();
            })
            .WithName("DeleteUser")
            .RequireAuthorization();

            // GET /api/users/me (em testes...)
            group.MapGet("/me", async (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Identity?.Name);
            });
        }
    }
}
