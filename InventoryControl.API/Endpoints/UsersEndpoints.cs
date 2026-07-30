using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;
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

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Created($"/api/users/{response.Value?.Id}", response.Value);
            })
            .WithName("CreateUser")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .RequireAuthorization(p => p.RequireRole("admin"));

            // PUT /api/users/{id}
            group.MapPut("/{id}", async (Guid id, CreateUserRequest request, UpdateUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("UpdateUser")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("admin"));

            // DELETE /api/users/{id}
            group.MapDelete("/{id}", async (Guid id, DeleteUserUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.NoContent();
            })
            .WithName("DeleteUser")
            .RequireAuthorization(p => p.RequireRole("admin"));

            //GET /api/users/username/{username}
            group.MapGet("/username/{username}", async ([FromQuery] string username, GetByUsernameUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(username);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetByUsername")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            //GET /api/users/email/{email}
            group.MapGet("/email/{email}", async ([FromQuery] string email, GetByEmailUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(email);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetByEmail")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // GET /api/users
            group.MapGet("/", async (GetAllUsersUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync();

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetAllUsers")
            .Produces<IEnumerable<UserResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));
        }
    }
}
