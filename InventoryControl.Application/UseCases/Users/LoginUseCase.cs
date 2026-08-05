using InventoryControl.Application.DTOs.Users;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Domain.Interfaces.Services;
using SecureIdentity.Password;

namespace InventoryControl.Application.UseCases.Users
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUseCase(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponse>?> ExecuteAsync(LoginRequest request)
        {
            var userValidation = new UserValidation(_userRepository);

            if (!userValidation.IsValidEmailFormat(request.Email))
                return Result<LoginResponse>.Failure(new Error("User.InvalidEmail", "O email informado é inválido."));

            var user = await _userRepository.GetByEmailAsync(Guid.Empty, request.Email);

            // Validar as credenciais do usuário (verificar se usuário foi encontrado, após pegar a senha e verificar o hash)
            if (user == null || !PasswordHasher.Verify(user.PasswordHash, request.Password))
                return Result<LoginResponse>.Failure(new Error("User.InvalidCredentials", "Credenciais inválidas."));

            if (PasswordHasher.Verify(user.PasswordHash, request.Password) && user.ForceChangePassword)
                return Result<LoginResponse>.Failure(new Error("User.ForceChangePassword", "Você deve alterar sua senha antes de continuar. Acesse o endpoint /api/users/change-password."));

            var token = _tokenService.GenerateToken(user);

            return Result<LoginResponse>.Success(
                new LoginResponse(token, user.Email));
        }
    }
}
