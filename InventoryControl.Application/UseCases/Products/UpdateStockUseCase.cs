using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces;

namespace InventoryControl.Application.UseCases.Products
{
    public class UpdateStockUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStockUseCase(IProductRepository productRepository, IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductWithCurrentStockResponse> ExecuteAsync(UpdateStockRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            MovementType movementType;

            if (product == null)
                throw new ArgumentException("Produto não encontrado.", nameof(request.ProductId));
                
            if (request.IsAddition)
            {
                product.AddToStock(request.Quantity);
                movementType = MovementType.Input;
            }
            else
            {
                product.RemoveFromStock(request.Quantity);
                movementType = MovementType.Output;
            }

            _productRepository.Update(product);

            // Registra o movimento de estoque
            var stockMovement = new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Type = movementType,
                Observation = request.Observation,
                UserId = Guid.Parse("4a29ec04-3203-44e2-a927-d23f421a0959") // Pegar o usuário logado no contexto da aplicação
            };
            await _stockMovementRepository.AddAsync(stockMovement);

            await _unitOfWork.CommitAsync();

            return new ProductWithCurrentStockResponse(
                product.Id,
                product.Name,
                product.Sku,
                product.Description,
                product.Price,
                product.CurrentStock
            );
        }
    }
}
