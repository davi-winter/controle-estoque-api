using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IStockMovementRepository : IBaseRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetStockMovementsAsync(int page, int pageSize);
        Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId, int page, int pageSize);
        Task<IEnumerable<StockMovement>> GetHistoryByUserIdAsync(Guid userId, int page, int pageSize);
        Task<IEnumerable<StockMovement>> GetHistoryByPeriodAsync(DateOnly startDate, DateOnly endDate, int page, int pageSize);
    }
}
