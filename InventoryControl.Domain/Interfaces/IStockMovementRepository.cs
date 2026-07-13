using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces
{
    public interface IStockMovementRepository : IBaseRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetHistoryByProductIdAsync(Guid productId);
    }
}
