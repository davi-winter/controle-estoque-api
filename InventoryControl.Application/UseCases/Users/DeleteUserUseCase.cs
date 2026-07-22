using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> ExecuteAsync(Guid userId)
        {
            var user = await _repository.GetByIdAsync(userId);

            if (user == null)
                return Result<bool>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

            _repository.Delete(user);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}
