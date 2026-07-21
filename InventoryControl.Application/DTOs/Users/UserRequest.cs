
namespace InventoryControl.Application.DTOs.Users
{
    public record CreateUserRequest(
        string Username,
        string Email,
        string Password,
        string Role);

    public record LoginRequest(string Email, string Password);
}
