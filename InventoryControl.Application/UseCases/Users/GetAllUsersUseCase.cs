using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Users
{
    public class GetAllUsersUseCase
    {
        private readonly IUserRepository _repository;

        public GetAllUsersUseCase(IUserRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<UserResponse>>> ExecuteAsync()
        {
            var users = await _repository.GetAllAsync("username");

            if (!users.Any())
                return Result<IEnumerable<UserResponse>>.Failure(new Error("User.NotFound", "Não há usuários cadastrados."));

            return Result<IEnumerable<UserResponse>>.Success(
                users.Select(u =>
                    new UserResponse(
                        u.Id,
                        u.Username,
                        u.Email,
                        u.Role,
                        u.CreatedAt
                    )
                ).ToList()
            );
        }
    }
}