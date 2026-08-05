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

        public async Task<Result<FirstLoginResponse>> ExecuteAsync(CreateUserRequest request)
        {
            var userValidation = new UserValidation(_repository);

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result<FirstLoginResponse>.Failure(new Error("User.UsernameRequired", "O nome de usuário é obrigatório."));

            if (request.Username.Length < 3 || request.Username.Length > 50)
                return Result<FirstLoginResponse>.Failure(new Error("User.InvalidUsernameLength", "O nome de usuário deve ter entre 3 e 50 caracteres."));

            if (!userValidation.IsUsernameUnique(Guid.Empty, request.Username))
                return Result<FirstLoginResponse>.Failure(new Error("User.UsernameExists", "O nome de usuário informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<FirstLoginResponse>.Failure(new Error("User.EmailRequired", "O email é obrigatório."));

            if (!userValidation.IsValidEmailFormat(request.Email))
                return Result<FirstLoginResponse>.Failure(new Error("User.InvalidEmail", "O email informado é inválido."));

            if (request.Email.Length > 100)
                return Result<FirstLoginResponse>.Failure(new Error("User.InvalidEmailLength", "O email deve ter no máximo 100 caracteres."));

            if (!userValidation.IsEmailUnique(Guid.Empty, request.Email))
                return Result<FirstLoginResponse>.Failure(new Error("User.EmailExists", "O email informado já está em uso."));

            if (string.IsNullOrWhiteSpace(request.Role))
                return Result<FirstLoginResponse>.Failure(new Error("User.RoleRequired", "A função é obrigatória."));

            if (!Enum.TryParse<User.UserRole>(request.Role, ignoreCase: true, out var userRole))
                return Result<FirstLoginResponse>.Failure(new Error("User.InvalidRole", "A função informada é inválida. Aplique um dos seguintes valores: 'Admin', 'Manager' ou 'Operator'"));

            var tempPassword = PasswordGenerator.Generate(8);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(tempPassword),
                Role = userRole.ToString().ToLower(),
                ForceChangePassword = true
            };

            await _repository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return Result<FirstLoginResponse>.Success(
                new FirstLoginResponse(
                    user.Id,
                    user.Username,
                    user.Email,
                    tempPassword,
                    user.Role,
                    user.CreatedAt
                )
            );
        }
    }
}
