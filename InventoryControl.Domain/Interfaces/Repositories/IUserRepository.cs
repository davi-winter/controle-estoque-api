using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(Guid id, string username);
        Task<User?> GetByEmailAsync(Guid id, string email);
    }
}
