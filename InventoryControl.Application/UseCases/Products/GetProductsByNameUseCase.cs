using InventoryControl.Application.DTOs.Products;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.Products
{
    public class GetProductsByNameUseCase
    {
        private readonly IProductRepository _repository;

        public GetProductsByNameUseCase(IProductRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<ProductWithCategoryResponse>>> ExecuteAsync(string name, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IEnumerable<ProductWithCategoryResponse>>.Failure(new Error("Product.NameRequired", "O nome do produto é obrigatório."));

            var products = await _repository.GetProductsByNameAsync(name, page, pageSize);

            return Result<IEnumerable<ProductWithCategoryResponse>>.Success(products
                .Select(p =>
                    new ProductWithCategoryResponse(
                        p.Id,
                        p.Name,
                        p.Sku,
                        p.Description,
                        p.Price,
                        p.Category!.Name)
                ).ToList()
            );
        }
    }
}
