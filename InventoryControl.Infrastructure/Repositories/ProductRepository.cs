using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetBySkuAsync(string sku)
            => await _dbSet.FirstOrDefaultAsync(p => p.Sku == sku);

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int limit)
            => await _dbSet
                .Where(p => p.CurrentStock <= limit)
                .AsNoTracking()
                .OrderBy(p => p.CurrentStock)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync(Guid categoryId)
            => await _dbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .ToListAsync();

        public async Task<bool> CategoryExistsAsync(Guid categoryId)
        {
            var category = await _dbSet.FirstOrDefaultAsync(p => p.CategoryId == categoryId);
            return category != null;
        }
    }
}
