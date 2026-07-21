using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IStockMovementRepository : IBaseRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId);
    }
}
