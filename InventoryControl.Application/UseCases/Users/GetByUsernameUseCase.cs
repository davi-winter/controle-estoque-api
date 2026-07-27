using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Users
{
    public class GetByUsernameUseCase
    {
        private readonly IUserRepository _repository;

        public GetByUsernameUseCase(IUserRepository repository)
            => _repository = repository;

        public async Task<Result<UserResponse>> ExecuteAsync(string username)
        {
            var user = await _repository.GetByUsernameAsync(Guid.Empty, username);

            if (user == null)
                return Result<UserResponse>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

            return Result<UserResponse>.Success(new UserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.CreatedAt));
        }
    }
}
