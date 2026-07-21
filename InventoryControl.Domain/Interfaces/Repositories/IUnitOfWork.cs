namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
