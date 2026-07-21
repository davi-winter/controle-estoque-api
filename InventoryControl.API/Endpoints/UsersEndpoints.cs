using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.UseCases.Categories;
using InventoryControl.Application.UseCases.Users;
using InventoryControl.Application.Validations;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace InventoryControl.API.Endpoints
{
    public static class UsersEndpoints
    {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users")
                           .WithTags("Users");

            // POST /api/users
            group.MapPost("/", async (CreateUserRequest request, CreateUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                return Results.Created($"/api/users/{response.Id}", response);
            })
            .WithName("CreateUser")
            .Produces<UserResponse>(StatusCodes.Status201Created);

            // POST /api/users/login
            group.MapPost("/login", async (LoginRequest request, LoginUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response.IsFailure)
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

                return Results.Ok(response.Value);
            })
            .WithName("Login")
            .Produces<LoginResponse>(StatusCodes.Status200OK);

            // GET /api/users/me (em testes...)
            group.MapGet("/me", async (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Identity?.Name);
            });
        }
    }
}
