using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces;
using InventoryControl.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Repositories
{
    public class StockMovementRepository : BaseRepository<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId)
            => await _dbSet
            .Where(sm => sm.ProductId == productId)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
