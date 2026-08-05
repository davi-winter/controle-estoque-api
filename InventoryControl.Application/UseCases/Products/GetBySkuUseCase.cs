using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetBySkuUseCase
    {
        private readonly IProductRepository _repository;

        public GetBySkuUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<Result<ProductResponse?>> ExecuteAsync(string sku)
        {
            var product = await _repository.GetBySkuAsync(Guid.Empty, sku);

            if (product == null)
                return Result<ProductResponse?>.Failure(new Error("Product.NotFound", "Produto não encontrado."));
                
            return Result<ProductResponse?>.Success(
                new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Sku,
                    product.Description,
                    product.Price,
                    product.CurrentStock
                )
            );
        }
    }
}
