namespace InventoryControl.Application.DTOs
{
    public record CreateProductRequest(
        string Name,
        string Sku,
        string Description,
        decimal Price,
        int CurrentStock,
        Guid CategoryId
    );
}
