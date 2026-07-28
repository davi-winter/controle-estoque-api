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

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Created($"/api/products/{response?.Value?.Id}", response?.Value);
            })
            .WithName("CreateProduct")
            .Produces<ProductResponse>(StatusCodes.Status201Created)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // PUT /api/products/{id}
            group.MapPut("/{id}", async (Guid id, CreateProductRequest request, UpdateProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("UpdateProduct")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // PATCH /api/products/update-stock
            group.MapPatch("/update-stock", async (UpdateStockRequest request, UpdateStockUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                return Results.Ok(response);
            })
            .RequireAuthorization(p => p.RequireRole("operator"));

            // DELETE /api/products/{id}
            group.MapDelete("/{id}", async (Guid id, DeleteProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.NoContent();
            })
            .WithName("DeleteProduct")
            .RequireAuthorization(p => p.RequireRole("admin"));

            //GET /api/products/{sku}
            group.MapGet("/{sku}", async ([FromQuery] string sku, GetBySkuUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(sku);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetBySku")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));

            // GET /api/products/low-stock
            group.MapGet("/low-stock", async ([FromQuery] int? limit, GetLowStockProductsUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(limit ?? 10);

                return Results.Ok(response);
            })
            .WithName("GetLowStockProducts")
            .Produces<IEnumerable<ProductWithCurrentStockResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            // GET /api/products/products-with-category
            group.MapGet("/products-with-category/{categoryId}", async ([FromQuery] Guid categoryId, GetProductsWithCategoryUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(categoryId);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetProductsWithCategory")
            .Produces<IEnumerable<ProductWithCategoryResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));
        }
    }
}
