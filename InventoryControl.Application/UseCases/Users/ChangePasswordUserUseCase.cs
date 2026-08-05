using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;
using SecureIdentity.Password;

namespace InventoryControl.Application.UseCases.Users
{
    public class ChangePasswordUserUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ChangePasswordResponse>> ExecuteAsync(ChangePasswordRequest request)
        {
            var userValidation = new UserValidation(_repository);

            var user = await _repository.GetByEmailAsync(Guid.Empty, request.Email);

            if (user == null)
                return Result<ChangePasswordResponse>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return Result<ChangePasswordResponse>.Failure(new Error("User.NewPasswordRequired", "A nova senha é obrigatória."));

            if (!userValidation.IsPasswordValid(request.NewPassword))
                return Result<ChangePasswordResponse>.Failure(new Error("User.InvalidNewPassword", "A nova senha informada é inválida. Deve conter pelo menos 8 caracteres, uma letra maiúscula, uma minúscula, um número e um caractere especial."));

            if (request.NewPassword.Length > 255)
                return Result<ChangePasswordResponse>.Failure(new Error("User.InvalidNewPasswordLength", "A nova senha deve ter no máximo 255 caracteres."));

            if (request.NewPassword == request.CurrentPassword)
                return Result<ChangePasswordResponse>.Failure(new Error("User.SamePassword", "A nova senha não pode ser igual à senha atual."));

            if (!PasswordHasher.Verify(user.PasswordHash, request.CurrentPassword))
                return Result<ChangePasswordResponse>.Failure(new Error("User.InvalidCurrentPassword", "A senha atual informada é inválida."));

            user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
            user.ForceChangePassword = false;

            _repository.Update(user);
            
            await _unitOfWork.CommitAsync();

            return Result<ChangePasswordResponse>.Success(
                new ChangePasswordResponse(
                    user.Email, 
                    "Senha alterada com sucesso. Acesse o endpoint /api/users/login para fazer login."
                )
            );
        }
    }
}
