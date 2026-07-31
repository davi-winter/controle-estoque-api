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

        public async Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId, int page, int pageSize)
            => await _dbSet
            .Where(sm => sm.ProductId == productId)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<IEnumerable<StockMovement>> GetHistoryByUserIdAsync(Guid userId, int page, int pageSize)
            => await _dbSet
            .Where(sm => sm.UserId == userId)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<IEnumerable<StockMovement>> GetHistoryByPeriodAsync(DateOnly startDate, DateOnly endDate, int page, int pageSize)
            => await _dbSet
            .Where(sm => DateOnly.FromDateTime(sm.MovedAt) >= startDate && DateOnly.FromDateTime(sm.MovedAt) <= endDate)
            .Include(m => m.User)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<IEnumerable<StockMovement>> GetStockMovementsAsync(int page, int pageSize)
            => await _dbSet
            .Include(m => m.User)
            .Skip(page * pageSize)
            .Take(pageSize)
            .OrderByDescending(sm => sm.MovedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
