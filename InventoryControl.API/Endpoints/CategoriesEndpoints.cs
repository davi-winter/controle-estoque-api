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

                return Results.Created($"/api/categories/{response.Id}", response);
            })
            .WithName("CreateCategory")
            .Produces<CategoryResponse>(StatusCodes.Status201Created);
        }
    }
}
