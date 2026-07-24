using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using SecureIdentity.Password;

namespace InventoryControl.Application.UseCases.Users
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserResponse>> ExecuteAsync(CreateUserRequest request)
        {
            var userValidation = new UserValidation(_repository);

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result<UserResponse>.Failure(new Error("User.UsernameRequired", "O nome de usuário é obrigatório."));

            if (request.Username.Length < 3 || request.Username.Length > 50)
                return Result<UserResponse>.Failure(new Error("User.InvalidUsernameLength", "O nome de usuário deve ter entre 3 e 50 caracteres."));

            if (!userValidation.IsUsernameUnique(request.Username))
                return Result<UserResponse>.Failure(new Error("User.UsernameExists", "O nome de usuário informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<UserResponse>.Failure(new Error("User.EmailRequired", "O email é obrigatório."));

            if (!userValidation.IsValidEmailFormat(request.Email))
                return Result<UserResponse>.Failure(new Error("User.InvalidEmail", "O email informado é inválido."));

            if (request.Email.Length > 100)
                return Result<UserResponse>.Failure(new Error("User.InvalidEmailLength", "O email deve ter no máximo 100 caracteres."));

            if (!userValidation.IsEmailUnique(request.Email))
                return Result<UserResponse>.Failure(new Error("User.EmailExists", "O email informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result<UserResponse>.Failure(new Error("User.PasswordRequired", "A senha é obrigatória."));

            if (!userValidation.IsPasswordValid(request.Password))
                return Result<UserResponse>.Failure(new Error("User.InvalidPassword", "A senha informada é inválida. Deve conter pelo menos 8 caracteres, uma letra maiúscula, uma minúscula, um número e um caractere especial."));

            if (request.Password.Length > 255)
                return Result<UserResponse>.Failure(new Error("User.InvalidPasswordLength", "A senha deve ter no máximo 255 caracteres."));

            if (string.IsNullOrWhiteSpace(request.Role))
                return Result<UserResponse>.Failure(new Error("User.RoleRequired", "A função é obrigatória."));

            if (!Enum.TryParse<User.UserRole>(request.Role, ignoreCase: true, out var userRole))
                return Result<UserResponse>.Failure(new Error("User.InvalidRole", "A função informada é inválida. Aplique um dos seguintes valores: 'Admin', 'Manager' ou 'Operator'"));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = userRole.ToString().ToLower()
            };

            await _repository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return Result<UserResponse>.Success(
                new UserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.CreatedAt
            ));
        }
    }
}
