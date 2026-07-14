using InventoryControl.Application.DTOs;
using InventoryControl.Application.UseCases.Products;

namespace InventoryControl.API.Endpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products")
                           .WithTags("Products");

            // POST /api/products
            group.MapPost("/", async (CreateProductRequest request, CreateProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                return Results.Created($"/api/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<ProductResponse>(StatusCodes.Status201Created);
        }
    }
}
