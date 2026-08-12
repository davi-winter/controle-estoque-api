using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Domain.Interfaces.Services;

namespace InventoryControl.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductUseCase(IProductRepository productRepository, IStockMovementRepository stockMovementRepository, 
            ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductResponse>> ExecuteAsync(CreateProductRequest request)
        {
            var productValidation = new ProductValidation(_productRepository);

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<ProductResponse>.Failure(new Error("Product.NameRequired", "O nome do produto é obrigatório."));

            if (request.Name.Length < 3 || request.Name.Length > 150)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidNameLength", "O nome do produto deve ter entre 3 e 150 caracteres."));

            if (string.IsNullOrWhiteSpace(request.Sku))
                return Result<ProductResponse>.Failure(new Error("Product.SkuRequired", "O SKU do produto é obrigatório."));

            if (request.Sku.Length < 8 || request.Sku.Length > 18)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidSkuLength", "O SKU do produto deve ter entre 8 e 18 caracteres."));

            if (!productValidation.IsValidSkuFormat(request.Sku))
                return Result<ProductResponse>.Failure(new Error("Product.InvalidSkuFormat", "O SKU do produto está em um formato inválido. Deve conter apenas letras, números e hífens."));

            if (!productValidation.IsSkuUnique(Guid.Empty, request.Sku))
                return Result<ProductResponse>.Failure(new Error("Product.SkuAlreadyExists", "O SKU do produto já existe."));

            if (request.Description.Length > 500)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidDescriptionLength", "A descrição do produto deve ter no máximo 500 caracteres."));

            if (request.Price <= 0)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidPrice", "O preço do produto deve ser um valor positivo."));

            if (request.InitialStock < 0)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidInitialStock", "O estoque inicial do produto deve ser um valor não negativo."));

            if (!productValidation.CategoryExists(request.CategoryId))
                return Result<ProductResponse>.Failure(new Error("Product.InvalidCategoryId", "A categoria do produto é inválida."));

            if (productValidation.InactiveCategory(request.CategoryId))
                return Result<ProductResponse>.Failure(new Error("Product.InactiveCategoryId", "A categoria do produto está inativa."));

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Sku = request.Sku.ToUpper(),
                Description = request.Description,
                Price = request.Price,
                CurrentStock = request.InitialStock,
                IsActive = request.IsActive,
                CategoryId = request.CategoryId
            };
            await _productRepository.AddAsync(product);

            // Se houver estoque inicial do produto faz o registro do movimento de estoque
            if (request.InitialStock > 0)
            {
                var stockMovement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = request.InitialStock,
                    Type = MovementType.Input,
                    Observation = "Estoque inicial do produto.",
                    UserId = _currentUserService.UserId  // Pega o usuário autenticado no contexto da aplicação
                };
                await _stockMovementRepository.AddAsync(stockMovement);
            }

            await _unitOfWork.CommitAsync();

            return Result<ProductResponse>.Success(
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
