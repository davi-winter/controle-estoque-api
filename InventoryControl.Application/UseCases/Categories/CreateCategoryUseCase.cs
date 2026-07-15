using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Categories
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryUseCase(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await _repository.AddAsync(category);
            await _unitOfWork.CommitAsync();

            return new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive
            );
        }
    }
}
