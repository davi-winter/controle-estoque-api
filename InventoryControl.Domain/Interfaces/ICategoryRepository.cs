using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
