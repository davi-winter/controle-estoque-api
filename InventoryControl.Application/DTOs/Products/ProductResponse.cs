namespace InventoryControl.Application.DTOs.Products
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string Sku,
        string Description, 
        decimal Price);

    public record ProductWithCurrentStockResponse(
        Guid Id,
        string Name,
        string Sku,
        string Description,
        decimal Price,
        int CurrentStock);

    public record ProductWithCategoryResponse(
        Guid Id,
        string Name,
        string Sku,
        string Description,
        decimal Price,
        string Category);
}
