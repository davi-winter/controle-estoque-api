using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Domain.Interfaces.Services;

namespace InventoryControl.Application.UseCases.StockMovements
{
    public class CreateStockMovementUseCase
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStockMovementUseCase(IStockMovementRepository stockMovementRepository, IProductRepository productRepository, 
            ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _stockMovementRepository = stockMovementRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductWithCurrentStockResponse>> ExecuteAsync(StockMovementRequest request)
        {
            MovementType movementType;
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
                return Result<ProductWithCurrentStockResponse>.Failure(new Error("Product.NotFound", "Produto não encontrado."));

            if (request.Quantity <= 0)
                return Result<ProductWithCurrentStockResponse>.Failure(new Error("Product.InvalidQuantity", "Quantidade inválida para o movimento de estoque."));

            if (!request.IsAddition && product.CurrentStock < request.Quantity)
                return Result<ProductWithCurrentStockResponse>.Failure(new Error("Product.InsufficientStock", "Estoque insuficiente para a saída do produto."));

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

            return Result<ProductWithCurrentStockResponse>.Success(new ProductWithCurrentStockResponse(
                product.Id,
                product.Name,
                product.Sku,
                product.Description,
                product.Price,
                product.CurrentStock
            ));
        }
    }
}
