using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetLowStockProductsUseCase
    {
        private readonly IProductRepository _repository;

        public GetLowStockProductsUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<ProductResponse>> ExecuteAsync(int minimumLimit = 10)
        {
            var products = await _repository.GetLowStockProductsAsync(minimumLimit);

            return products
                .Select(p => 
                    new ProductResponse(
                        p.Id,
                        p.Name,
                        p.Sku,
                        p.Description,
                        p.Price,
                        p.CurrentStock)
                ).ToList();
        }
    }
}
