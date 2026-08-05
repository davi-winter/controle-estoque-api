using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Categories
{
    public class UpdateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryUseCase(ICategoryRepository categoryRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryResponse>> ExecuteAsync(Guid categoryId, CreateCategoryRequest request)
        {
            var categoryValidation = new CategoryValidation(_categoryRepository);

            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                return Result<CategoryResponse>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<CategoryResponse>.Failure(new Error("Category.NameRequired", "O nome da categoria é obrigatório."));

            if (categoryValidation.CategoryByNameExists(categoryId, request.Name))
                return Result<CategoryResponse>.Failure(new Error("Category.AlreadyExists", "Já existe uma categoria com esse nome."));

            if (request.Name.Length < 3 || request.Name.Length > 100)
                return Result<CategoryResponse>.Failure(new Error("Category.InvalidNameLength", "O nome da categoria deve ter entre 3 e 100 caracteres."));

            if (string.IsNullOrWhiteSpace(request.Description))
                return Result<CategoryResponse>.Failure(new Error("Category.DescriptionRequired", "A descrição da categoria é obrigatória."));

            if (request.Description.Length > 255)
                return Result<CategoryResponse>.Failure(new Error("Category.InvalidDescriptionLength", "A descrição da categoria deve ter no máximo 255 caracteres."));

            category.Name = request.Name;
            category.Description = request.Description;
            category.IsActive = request.IsActive;

            _categoryRepository.Update(category);

            // Atualiza o status de todos os produtos associados à categoria
            await _productRepository.UpdateStatusProductsByCategoryIdAsync(category.Id, request.IsActive);

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
