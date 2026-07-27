using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.Validations
{
    public class CategoryValidation
    {
        private readonly ICategoryRepository _repository;

        public CategoryValidation(ICategoryRepository repository)
            => _repository = repository;

        public bool CategoryByNameExists(Guid id, string categoryName)
            => _repository.ExistsByNameAsync(id, categoryName).Result;
    }
}
