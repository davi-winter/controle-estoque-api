using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.UseCases.StockMovements;

namespace InventoryControl.API.Endpoints
{
    public static class StockMovementsEndpoints
    {
        public static void MapStockMovementsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stock-movements")
                           .WithTags("StockMovements");

            // POST /api/stock-movements
            group.MapPost("/", async (StockMovementRequest request, CreateStockMovementUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(request);

                if (response.IsFailure)
                {
                    if (response.Error?.Code == "Product.NotFound")
                        return Results.NotFound(response.Error);
                    else
                        return Results.BadRequest(response.Error);
                }

                return Results.Ok(response.Value);
            })
            .WithName("CreateStockMovement")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("operator"));
        }
    }
}
