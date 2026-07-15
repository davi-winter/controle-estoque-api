namespace InventoryControl.Application.DTOs.Categories
{
    public record CreateCategoryRequest(
        string Name,
        string Description,
        bool IsActive
    );
}
