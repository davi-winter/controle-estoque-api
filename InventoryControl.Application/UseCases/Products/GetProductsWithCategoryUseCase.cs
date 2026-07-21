using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetProductsWithCategoryUseCase
    {
        private readonly IProductRepository _repository;

        public GetProductsWithCategoryUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<ProductWithCategoryResponse>> ExecuteAsync(Guid categoryId)
        {
            var products = await _repository.GetProductsWithCategoryAsync(categoryId);

            return products
                .Select(p =>
                    new ProductWithCategoryResponse(
                        p.Id,
                        p.Name,
                        p.Sku,
                        p.Description,
                        p.Price,
                        p.Category!.Name)
                ).ToList();
        }
    }
}
