using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetLowStockProductsUseCase
    {
        private readonly IProductRepository _repository;

        public GetLowStockProductsUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<ProductResponse>> ExecuteAsync(int limit = 10, int page = 0, int pageSize = 25)
        {
            var products = await _repository.GetLowStockProductsAsync(limit, page, pageSize);

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
