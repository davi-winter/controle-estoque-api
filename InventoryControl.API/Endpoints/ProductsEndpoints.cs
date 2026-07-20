using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.UseCases.Products;
using Microsoft.AspNetCore.Mvc;

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

            // PUT /api/products/{id}
            group.MapPut("/{id}", async (Guid id, CreateProductRequest request, UpdateProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<ProductResponse>(StatusCodes.Status200OK);

            // PATCH /api/products/update-stock
            group.MapPatch("/update-stock", async (UpdateStockRequest request, UpdateStockUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                return Results.Ok(response);
            });

            // DELETE /api/products/{id}
            group.MapDelete("/{id}", async (Guid id, DeleteProductUseCase useCase) =>
            {
                await useCase.ExecuteAsync(id);

                return Results.NoContent();
            });

            //GET /api/products/{sku}
            group.MapGet("/{sku}", async ([FromQuery] string sku, GetBySkuUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(sku);

                return response is null
                    ? Results.NotFound()
                    : Results.Ok(response);
            });

            // GET /api/products/low-stock
            group.MapGet("/low-stock", async ([FromQuery] int? limit, GetLowStockProductsUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(limit ?? 10);

                return Results.Ok(response);
            })
            .WithName("GetLowStockProducts")
            .Produces<IEnumerable<ProductWithCurrentStockResponse>>(StatusCodes.Status200OK);

            // GET /api/products/products-with-category
            group.MapGet("/products-with-category/{categoryId}", async ([FromQuery] Guid categoryId, GetProductsWithCategoryUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(categoryId);

                return Results.Ok(response);
            })
            .WithName("GetProductsWithCategory")
            .Produces<IEnumerable<ProductWithCategoryResponse>>(StatusCodes.Status200OK);
        }
    }
}
