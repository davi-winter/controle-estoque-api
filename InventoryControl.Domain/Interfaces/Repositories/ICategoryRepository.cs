using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
