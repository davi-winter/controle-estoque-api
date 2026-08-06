using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Users
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserResponse>> ExecuteAsync(Guid userId, CreateUserRequest request)
        {
            var userValidation = new UserValidation(_repository);

            var user = await _repository.GetByIdAsync(userId);

            if (user == null)
                return Result<UserResponse>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result<UserResponse>.Failure(new Error("User.UsernameRequired", "O nome de usuário é obrigatório."));

            if (!userValidation.IsUsernameUnique(userId, request.Username))
                return Result<UserResponse>.Failure(new Error("User.UsernameExists", "O nome de usuário informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<UserResponse>.Failure(new Error("User.EmailRequired", "O email é obrigatório."));

            if (!userValidation.IsValidEmailFormat(request.Email))
                return Result<UserResponse>.Failure(new Error("User.InvalidEmail", "O email informado é inválido."));

            if (!userValidation.IsEmailUnique(userId, request.Email))
                return Result<UserResponse>.Failure(new Error("User.EmailExists", "O email informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Role))
                return Result<UserResponse>.Failure(new Error("User.RoleRequired", "A função é obrigatória."));

            if (!Enum.TryParse<User.UserRole>(request.Role, ignoreCase: true, out var userRole))
                return Result<UserResponse>.Failure(new Error("User.InvalidRole", "A função informada é inválida. Aplique um dos seguintes valores: 'Admin', 'Manager' ou 'Operator'"));

            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role.ToLower();

            _repository.Update(user);
            await _unitOfWork.CommitAsync();

            return Result<UserResponse>.Success(
                new UserResponse(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    user.CreatedAt
                )
            );
        }
    }
}
