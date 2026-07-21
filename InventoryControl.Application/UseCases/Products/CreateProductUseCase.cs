using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

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
            await _unitOfWork.CommitAsync();

            return new ProductResponse(
                product.Id,
                product.Name,
                product.Sku,
                product.Description,
                product.Price
            );
        }
    }
}
