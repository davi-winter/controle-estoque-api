namespace InventoryControl.Application.DTOs.StockMovements
{
    public record StockMovementRequest(
        Guid ProductId,
        int Quantity,
        bool IsAddition,
        string Observation
    );
}
