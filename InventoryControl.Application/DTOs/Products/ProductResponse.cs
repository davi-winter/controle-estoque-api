namespace InventoryControl.Application.DTOs.Products
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string Description, 
        decimal Price);
}
