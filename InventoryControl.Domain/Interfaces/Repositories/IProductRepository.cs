using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product?> GetBySkuAsync(Guid id, string sku);
        Task<bool> CategoryExistsAsync(Guid categoryId);
        Task<bool> InactiveCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int limit, int page, int pageSize);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, bool includeInactive, int page = 0, int pageSize = 10);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name, int page, int pageSize);
        Task UpdateStatusProductsByCategoryIdAsync(Guid categoryId, bool isActive);
    }
}
