using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace InventoryControl.Infrastructure.Context.Data.Decorators
{
    public class CachedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository _innerRepository;
        private readonly IMemoryCache _memoryCache;

        public CachedCategoryRepository(ICategoryRepository innerRepository, IMemoryCache memoryCache)
        {
            _innerRepository = innerRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"categoryId_{id}";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return await _innerRepository.GetByIdAsync(id);
            });
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? sortBy = null, bool ascending = true)
        {
            string cacheKey = "categories";

            var categories = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return await _innerRepository.GetAllAsync(sortBy, ascending);
            });

            return categories!;
        }

        public async Task<bool> ExistsByNameAsync(Guid id, string name)
        {
            string cacheKey = $"categoryExists_{id}_{name.ToLower()}";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return await _innerRepository.ExistsByNameAsync(id, name);
            });
        }

        public Task AddAsync(Category category)
            => _innerRepository.AddAsync(category);

        public void Update(Category category)
            => _innerRepository.Update(category);

        public void Delete(Category category)
            => _innerRepository.Delete(category);

        public async Task SaveChangesAsync()
            => await _innerRepository.SaveChangesAsync();
    }
}
