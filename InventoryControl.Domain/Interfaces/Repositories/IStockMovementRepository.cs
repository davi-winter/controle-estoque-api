using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IStockMovementRepository : IBaseRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId);
        Task<IEnumerable<StockMovement>> GetHistoryByUserIdAsync(Guid userId);
        Task<IEnumerable<StockMovement>> GetHistoryByPeriodAsync(DateOnly startDate, DateOnly endDate);
    }
}
