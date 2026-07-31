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

        public async Task<Product?> GetBySkuAsync(Guid id, string sku)
            => await _dbSet.FirstOrDefaultAsync(p => p.Id != id && p.Sku == sku);

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int limit, int page, int pageSize)
            => await _dbSet
                .Where(p => p.CurrentStock <= limit)
                .AsNoTracking()
                .OrderBy(p => p.CurrentStock)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, int page, int pageSize)
            => await _dbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<bool> CategoryExistsAsync(Guid categoryId)
        {
            var category = await _dbSet.FirstOrDefaultAsync(p => p.CategoryId == categoryId);
            return category != null;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, int page, int pageSize)
            => await _dbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{name}%"))
                .OrderBy(p => p.Name)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
