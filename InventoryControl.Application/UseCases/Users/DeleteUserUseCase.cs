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

            if (user.StockMovements.Any())
                return Result<bool>.Failure(new Error("User.HasStockMovements", "O usuário possui movimentações de estoque associadas."));

            _repository.Delete(user);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}
