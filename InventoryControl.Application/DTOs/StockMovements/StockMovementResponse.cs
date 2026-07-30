using InventoryControl.Domain.Entities;

namespace InventoryControl.Application.DTOs.StockMovements
{
    public record StockMovementResponse(
        Guid Id,
        Guid ProductId,
        string ProductName,
        int Quantity,
        string Type,
        DateTime MovedAt,
        string Observation,
        Guid UserId,
        string UserName
    );
}