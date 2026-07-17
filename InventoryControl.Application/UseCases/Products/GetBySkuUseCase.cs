using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetBySkuUseCase
    {
        private readonly IProductRepository _repository;

        public GetBySkuUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<ProductResponse?> ExecuteAsync(string sku)
        {
            var product = await _repository.GetBySkuAsync(sku);

            return product is null 
                ? null 
                : new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Sku,
                    product.Description,
                    product.Price);
        }
    }
}
