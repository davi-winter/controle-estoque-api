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
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                return Result<LoginResponse>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

            // Validar as credenciais do usuário (pegar a senha e verificar o hash)
            if (!PasswordHasher.Verify(user.PasswordHash, request.Password))
                return Result<LoginResponse>.Failure(new Error("User.InvalidCredentials", "Credenciais inválidas."));

            var token = _tokenService.GenerateToken(user);

            return Result<LoginResponse>.Success(
                new LoginResponse(token, user.Email));
        }
    }
}
