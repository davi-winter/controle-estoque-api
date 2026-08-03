namespace InventoryControl.Application.DTOs.Products
{
    public record CreateProductRequest(
        string Name,
        string Sku,
        string Description,
        decimal Price,
        int InitialStock,
        bool IsActive,
        Guid CategoryId
    );

    public record UpdateProductRequest(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    bool IsActive,
    Guid CategoryId
    );
}
