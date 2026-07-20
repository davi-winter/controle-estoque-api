namespace InventoryControl.Application.DTOs.Products
{
    public record CreateProductRequest(
        string Name,
        string Sku,
        string Description,
        decimal Price,
        int CurrentStock,
        Guid CategoryId
    );

    public record UpdateStockRequest(
        Guid ProductId,
        int Quantity,
        bool IsAddition,
        string Observation
    );
}
