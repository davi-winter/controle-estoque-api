using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Categories
{
    public class DeleteCategoryUseCase
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> ExecuteAsync(Guid categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);

            if (category == null)
                return Result<bool>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

            if (category.Products.Any())
                return Result<bool>.Failure(new Error("Category.HasProducts", "Não é possível excluir uma categoria que possui produtos."));

            _repository.Delete(category);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}
