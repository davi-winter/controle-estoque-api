using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Users
{
    public class GetByEmailUseCase
    {
        private readonly IUserRepository _repository;

        public GetByEmailUseCase(IUserRepository repository)
            => _repository = repository;

        public async Task<Result<UserResponse>> ExecuteAsync(string email)
        {
            var userValidation = new UserValidation(_repository);

            if (!userValidation.IsValidEmailFormat(email))
                return Result<UserResponse>.Failure(new Error("User.InvalidEmail", "O email informado é inválido."));

            var user = await _repository.GetByEmailAsync(email);

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
