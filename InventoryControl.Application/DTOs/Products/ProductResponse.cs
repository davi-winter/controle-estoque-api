namespace InventoryControl.Application.DTOs.Products
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string Sku,
        string Description, 
        decimal Price,
        int CurrentStock
    );

    public record ProductStatusResponse(
        Guid Id,
        string Name,
        string Sku,
        bool IsActive
    );

    public record ProductWithCategoryResponse(
        Guid Id,
        string Name,
        string Sku,
        string Description,
        decimal Price,
        string Category);
}
