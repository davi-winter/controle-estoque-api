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
            group.MapPut("/{id}", async (Guid id, UpdateProductRequest request, UpdateProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(id, request);

                if (response.IsFailure)
                    return Results.BadRequest(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("UpdateProduct")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

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

            // PATCH /api/products/{id}/status
            group.MapPatch("/{id}/status", async ([FromBody] ChangeStatusProductRequest request, ChangeStatusProductUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("ChangeStatusProduct")
            .Produces<ProductStatusResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

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
            group.MapGet("/low-stock", async (
                [FromQuery] int? limit, 
                [FromQuery] int? page, 
                [FromQuery] int? pageSize, 
                GetLowStockProductsUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(limit ?? 10, page ?? 0, pageSize ?? 25);

                return Results.Ok(response);
            })
            .WithName("GetLowStockProducts")
            .Produces<IEnumerable<ProductResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));

            // GET /api/products/products-by-category
            group.MapGet("/products-by-category/{categoryId}", async (
                [FromQuery] Guid categoryId,
                [FromQuery] bool? includeInactive,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetProductsByCategoryIdUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(categoryId, includeInactive ?? false, page ?? 0, pageSize ?? 25);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetProductsByCategoryId")
            .Produces<IEnumerable<ProductWithCategoryResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));

            // GET /api/products/products-by-name
            group.MapGet("/products-by-name/{name}", async (
                [FromQuery] string name,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetProductsByNameUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(name, page ?? 0, pageSize ?? 25);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetProductsByName")
            .Produces<IEnumerable<ProductWithCategoryResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));
        }
    }
}
