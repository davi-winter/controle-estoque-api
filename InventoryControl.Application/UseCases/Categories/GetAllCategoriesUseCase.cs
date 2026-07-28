using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Categories
{
    public class GetAllCategoriesUseCase
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoriesUseCase(ICategoryRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<CategoryResponse>>> ExecuteAsync()
        {
            var categories = await _repository.GetAllAsync("name");

            if (!categories.Any())
                return Result<IEnumerable<CategoryResponse>>.Failure(new Error("Category.NotFound", "Não há categorias cadastradas."));

            return Result<IEnumerable<CategoryResponse>>.Success(
                categories.Select(c => new CategoryResponse(
                    c.Id,
                    c.Name,
                    c.Description,
                    c.IsActive)
                ).ToList());
        }
    }
}