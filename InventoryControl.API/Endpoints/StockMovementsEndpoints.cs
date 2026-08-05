using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.UseCases.StockMovements;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

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

            // GET /api/stock-movements
            group.MapGet("/", async (
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetStockMovementsUseCase useCase) =>
            {
                //if (ClaimsPrincipal.Current?.IsInRole("manager") != true)
                //    return Results.Forbid();

                var response = await useCase.ExecuteAsync(page ?? 0, pageSize ?? 25);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetStockMovements")
            .Produces<IEnumerable<StockMovementResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            //GET /api/stock-movements/get-history-by-product-id/{productId}
            group.MapGet("/get-history-by-product-id/{productId}", async (
                [FromQuery] Guid productId,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetHistoryByProductIdUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(productId, page ?? 0, pageSize ?? 25);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetHistoryByProductId")
            .Produces<IEnumerable<StockMovementResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            //GET /api/stock-movements/get-history-by-user-id/{userId}
            group.MapGet("/get-history-by-user-id/{userId}", async (
                [FromQuery] Guid userId,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetHistoryByUserIdUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(userId, page ?? 0, pageSize ?? 25);

                if (response.IsFailure)
                    return Results.NotFound(response.Error);

                return Results.Ok(response.Value);
            })
            .WithName("GetHistoryByUserId")
            .Produces<IEnumerable<StockMovementResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));

            //GET /api/stock-movements/get-history-by-period
            group.MapGet("/get-history-by-period", async (
                [FromQuery, Description("AAAA-MM-DD")] DateOnly startDate, 
                [FromQuery, Description("AAAA-MM-DD")] DateOnly endDate,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                GetHistoryByPeriodUseCase useCase) =>
            {
                var response = await useCase.ExecuteAsync(startDate, endDate, page ?? 0, pageSize ?? 25);

                if (response?.IsFailure == true)
                {
                    switch (response.Error?.Code)
                    {
                        case "StockMovement.NotFound":
                            return Results.NotFound(response.Error);
                        default:
                            return Results.BadRequest(response.Error);
                    }
                }

                return Results.Ok(response?.Value);
            })
            .WithName("GetHistoryByPeriod")
            .Produces<IEnumerable<StockMovementResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(p => p.RequireRole("manager"));
        }
    }
}
