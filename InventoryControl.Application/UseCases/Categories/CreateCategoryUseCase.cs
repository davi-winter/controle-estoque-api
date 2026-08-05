using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;

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

        public async Task<Result<CategoryResponse>> ExecuteAsync(CreateCategoryRequest request)
        {
            var categoryValidation = new CategoryValidation(_repository);

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<CategoryResponse>.Failure(new Error("Category.NameRequired", "O nome da categoria é obrigatório."));

            if (request.Name.Length < 3 || request.Name.Length > 100)
                return Result<CategoryResponse>.Failure(new Error("Category.InvalidNameLength", "O nome da categoria deve ter entre 3 e 100 caracteres."));

            if (categoryValidation.CategoryByNameExists(Guid.Empty, request.Name))
                return Result<CategoryResponse>.Failure(new Error("Category.AlreadyExists", "Já existe uma categoria com esse nome."));

            if (string.IsNullOrWhiteSpace(request.Description))
                return Result<CategoryResponse>.Failure(new Error("Category.DescriptionRequired", "A descrição da categoria é obrigatória."));

            if (request.Description.Length > 255)
                return Result<CategoryResponse>.Failure(new Error("Category.InvalidDescriptionLength", "A descrição da categoria deve ter no máximo 255 caracteres."));

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await _repository.AddAsync(category);
            await _unitOfWork.CommitAsync();

            return Result<CategoryResponse>.Success(
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
