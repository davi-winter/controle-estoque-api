using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Products
{
    public class UpdateProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> ExecuteAsync(Guid productId, CreateProductRequest request)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null)
                throw new ArgumentException("Produto não encontrado.", nameof(productId));

            product.Name = request.Name;
            product.Sku = request.Sku;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CurrentStock = request.CurrentStock;
            product.CategoryId = request.CategoryId;

            _repository.Update(product);
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
