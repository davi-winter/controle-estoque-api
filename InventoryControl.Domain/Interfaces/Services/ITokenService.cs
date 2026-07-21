using InventoryControl.Domain.Entities;

namespace InventoryControl.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
