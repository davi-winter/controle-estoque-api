namespace InventoryControl.Application.DTOs.Users
{
    public record FirstLoginResponse(
        Guid Id,
        string Username,
        string Email,
        string TempPassword,
        string Role,
        DateTime CreatedAt
    );

    public record ChangePasswordResponse(
        string Email,
        string Message
    );

    public record LoginResponse(
        string Token, 
        string Email
    );

    public record UserResponse(
        Guid Id,
        string Username,
        string Email,
        string Role,
        DateTime CreatedAt
    );
}
