using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Categories
{
    public class GetByCategoryIdUseCase
    {
        private readonly ICategoryRepository _repository;

        public GetByCategoryIdUseCase(ICategoryRepository repository)
            => _repository = repository;

        public async Task<Result<CategoryResponse?>> ExecuteAsync(Guid categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);

            if (category == null)
                return Result<CategoryResponse?>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

            return Result<CategoryResponse?>.Success(
                new CategoryResponse(
                    category.Id,
                    category.Name,
                    category.Description,
                    category.IsActive
                )
            );
        }
    }
}
