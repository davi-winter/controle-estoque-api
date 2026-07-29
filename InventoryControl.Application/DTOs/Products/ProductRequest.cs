namespace InventoryControl.Application.DTOs.Products
{
    public record CreateProductRequest(
        string Name,
        string Sku,
        string Description,
        decimal Price,
        int InitialStock,
        Guid CategoryId
    );

    public record UpdateProductRequest(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    Guid CategoryId
    );
}
