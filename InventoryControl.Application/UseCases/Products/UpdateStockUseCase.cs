using InventoryControl.Application.DTOs.Products;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Domain.Interfaces.Services;

namespace InventoryControl.Application.UseCases.Products
{
    public class UpdateStockUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStockUseCase(IProductRepository productRepository, IStockMovementRepository stockMovementRepository, 
            ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
            _currentUserService = currentUserService;
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
                UserId = _currentUserService.UserId  // Pegar o usuário autenticado no contexto da aplicação
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
