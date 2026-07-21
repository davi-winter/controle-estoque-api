using InventoryControl.Application.DTOs.Products;
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

        public async Task<bool> ExecuteAsync(Guid productId)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null)
                throw new ArgumentException("Produto não encontrado.", nameof(productId));

            _repository.Delete(product);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
