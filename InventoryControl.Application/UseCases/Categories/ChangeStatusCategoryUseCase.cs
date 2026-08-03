using InventoryControl.Application.DTOs.Categories;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Categories
{
    public class ChangeStatusCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusCategoryUseCase(ICategoryRepository categoryRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryResponse>> ExecuteAsync(ChangeStatusCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
                return Result<CategoryResponse>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

            category.IsActive = request.IsActive;

            _categoryRepository.Update(category);

            // Atualiza o status de todos os produtos associados à categoria
            await _productRepository.UpdateStatusProductsByCategoryIdAsync(category.Id, request.IsActive);

            await _unitOfWork.CommitAsync();

            return Result<CategoryResponse>.Success(new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive
            ));
        }
    }
}
