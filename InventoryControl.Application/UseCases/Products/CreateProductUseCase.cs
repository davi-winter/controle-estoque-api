using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using System.Net;
using System.Text.Json;

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

        public async Task<Result<ProductResponse>> ExecuteAsync(CreateProductRequest request)
        {
            var productValidation = new ProductValidation(_repository);

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

            if (!productValidation.IsSkuUnique(request.Sku))
                return Result<ProductResponse>.Failure(new Error("Product.SkuAlreadyExists", "O SKU do produto já existe."));

            if (request.Description.Length > 500)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidDescriptionLength", "A descrição do produto deve ter no máximo 500 caracteres."));

            if (request.Price <= 0)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidPrice", "O preço do produto deve ser um valor positivo."));

            if (request.CurrentStock < 0)
                return Result<ProductResponse>.Failure(new Error("Product.InvalidCurrentStock", "O estoque atual do produto deve ser um valor não negativo."));

            if (!productValidation.CategoryExists(request.CategoryId))
                return Result<ProductResponse>.Failure(new Error("Product.InvalidCategoryId", "A categoria do produto é inválida."));

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Sku = request.Sku.ToUpper(),
                Description = request.Description,
                Price = request.Price,
                CurrentStock = request.CurrentStock,
                CategoryId = request.CategoryId
            };

            await _repository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return Result<ProductResponse>.Success(new ProductResponse(
                product.Id,
                product.Name,
                product.Sku,
                product.Description,
                product.Price
            ));
        }
    }
}
