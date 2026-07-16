using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product?> GetBySkuAsync(string sku);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int limit);
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
    }
}
