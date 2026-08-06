namespace InventoryControl.Domain.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        IEnumerable<string> Roles { get; }
    }
}
