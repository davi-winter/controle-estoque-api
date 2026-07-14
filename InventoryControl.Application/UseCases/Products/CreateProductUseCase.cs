using InventoryControl.Application.DTOs;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _repository;

        public CreateProductUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<ProductResponse> ExecuteAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Sku = request.Sku,
                Description = request.Description,
                Price = request.Price,
                CurrentStock = request.CurrentStock,
                CategoryId = request.CategoryId
            };

            await _repository.AddAsync(product);

            return new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price
            );
        }
    }
}
