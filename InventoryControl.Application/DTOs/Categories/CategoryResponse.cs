namespace InventoryControl.Application.DTOs.Categories
{
    public record CategoryResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsActive);
}
