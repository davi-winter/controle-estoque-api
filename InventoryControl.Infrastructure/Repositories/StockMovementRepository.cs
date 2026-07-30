using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
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

        public async Task<IEnumerable<StockMovement>> GetHistoryByUserIdAsync(Guid userId)
            => await _dbSet
            .Where(sm => sm.UserId == userId)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .ToListAsync();

        public async Task<IEnumerable<StockMovement>> GetHistoryByPeriodAsync(DateOnly startDate, DateOnly endDate)
            => await _dbSet
            .Where(sm => DateOnly.FromDateTime(sm.MovedAt) >= startDate && DateOnly.FromDateTime(sm.MovedAt) <= endDate)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
