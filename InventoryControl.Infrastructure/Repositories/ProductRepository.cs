using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces;
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

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
            => await _dbSet.Include(p => p.Category).AsNoTracking().ToListAsync();
    }
}
