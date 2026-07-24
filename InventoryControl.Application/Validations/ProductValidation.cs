using InventoryControl.Domain.Interfaces.Repositories;
using System.Text.RegularExpressions;

namespace InventoryControl.Application.Validations
{
    public class ProductValidation
    {
        private readonly IProductRepository _repository;
        
        public ProductValidation(IProductRepository repository)
            => _repository = repository;

        public bool IsSkuUnique(string sku)
        {
            var product = _repository.GetBySkuAsync(sku).Result;
            return product == null;
        }

        public bool IsValidSkuFormat(string sku)
        {
            var pattern = @"^[a-zA-Z0-9-]+$";
            return Regex.IsMatch(sku, pattern, RegexOptions.IgnoreCase);
        }

        //public bool IsValidPriceFormat(decimal price)
        //{
        //    var pattern = @"^\d+([,\.]\d{2})?$";
        //    return Regex.IsMatch(price.ToString("F2"), pattern, RegexOptions.IgnoreCase);
        //}

        public bool CategoryExists(Guid categoryId)
            => _repository.CategoryExistsAsync(categoryId).Result;
    }
}
