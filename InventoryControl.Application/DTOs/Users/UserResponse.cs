namespace InventoryControl.Application.DTOs.Users
{
    public record LoginResponse(string Token, string Email
    );

    public record UserResponse(
        Guid Id,
        string Username,
        string Email,
        string Role,
        DateTime CreatedAt
    );
}
