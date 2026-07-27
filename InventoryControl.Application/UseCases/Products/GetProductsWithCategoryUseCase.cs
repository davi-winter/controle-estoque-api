using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetProductsWithCategoryUseCase
    {
        private readonly IProductRepository _repository;

        public GetProductsWithCategoryUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<ProductWithCategoryResponse>>> ExecuteAsync(Guid categoryId)
        {
            var productValidation = new ProductValidation(_repository);

            var products = await _repository.GetProductsWithCategoryAsync(categoryId);

            if (!productValidation.CategoryExists(categoryId))
                return Result<IEnumerable<ProductWithCategoryResponse>>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));   

            return Result<IEnumerable<ProductWithCategoryResponse>>.Success(products
                .Select(p =>
                    new ProductWithCategoryResponse(
                        p.Id,
                        p.Name,
                        p.Sku,
                        p.Description,
                        p.Price,
                        p.Category!.Name)
                ).ToList());
        }
    }
}
