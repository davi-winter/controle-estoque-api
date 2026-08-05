
namespace InventoryControl.Application.DTOs.Users
{
    public record CreateUserRequest(
        string Username,
        string Email,
        string Role);

    public record ChangePasswordRequest(
        string Email,
        string CurrentPassword,
        string NewPassword
    );

    public record LoginRequest(
        string Email, 
        string Password
     );
}
