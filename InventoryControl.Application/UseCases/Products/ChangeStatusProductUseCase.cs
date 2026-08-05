using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class ChangeStatusProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductStatusResponse>> ExecuteAsync(ChangeStatusProductRequest request)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product == null)
                return Result<ProductStatusResponse>.Failure(new Error("Product.NotFound", "Produto não encontrado."));

            product.IsActive = request.IsActive;

            _repository.Update(product);
            await _unitOfWork.CommitAsync();

            return Result<ProductStatusResponse>.Success(
                new ProductStatusResponse(
                    product.Id,
                    product.Name,
                    product.Sku,
                    product.IsActive
                )
            );
        }
    }
}
