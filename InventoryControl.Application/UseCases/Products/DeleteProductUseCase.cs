using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class DeleteProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> ExecuteAsync(Guid productId)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null)
                return Result<bool>.Failure(new Error("Product.NotFound", "Produto não encontrado."));

            if (product.StockMovements.Any())
                return Result<bool>.Failure(new Error("Product.HasStockMovements", "Não é possível excluir um produto que possui movimentações de estoque."));

            _repository.Delete(product);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}
