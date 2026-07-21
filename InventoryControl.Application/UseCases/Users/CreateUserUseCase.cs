using InventoryControl.Application.DTOs.Users;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using SecureIdentity.Password;

namespace InventoryControl.Application.UseCases.Categories
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

        public async Task<UserResponse> ExecuteAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = request.Role
            };

            await _repository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return new UserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.CreatedAt
            );
        }
    }
}
