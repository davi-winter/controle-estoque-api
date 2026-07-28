using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.UseCases.Categories;

namespace InventoryControl.API.Endpoints
{
    public static class CategoriesEndpoints
    {
        public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories")
                           .WithTags("Categories");

            // POST /api/categories
            group.MapPost("/", async (CreateCategoryRequest request, CreateCategoryUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Created($"/api/categories/{response.Value?.Id}", response.Value);
            })
            .WithName("CreateCategory")
            .Produces<CategoryResponse>(StatusCodes.Status201Created)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // PUT /api/categories/{id}
            group.MapPut("/{id}", async (Guid id, CreateCategoryRequest request, UpdateCategoryUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("UpdateCategory")
            .Produces<CategoryResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // DELETE /api/categories/{id}
            group.MapDelete("/{id}", async (Guid id, DeleteCategoryUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.NoContent();
            })
            .WithName("DeleteCategory")
            .RequireAuthorization(p => p.RequireRole("admin"));

            //GET /api/categories/{id}
            group.MapGet("/{id}", async (Guid id, GetByCategoryIdUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetByCategoryId")
            .Produces<CategoryResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));

            // GET /api/categories
            group.MapGet("/", async (GetAllCategoriesUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync();

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetAllCategories")
            .Produces<IEnumerable<CategoryResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));
        }
    }
}
