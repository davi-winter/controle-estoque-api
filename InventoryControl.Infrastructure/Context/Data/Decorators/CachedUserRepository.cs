using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace InventoryControl.Infrastructure.Context.Data.Decorators
{
    public class CachedUserRepository : IUserRepository
    {
        private readonly IUserRepository _innerRepository;
        private readonly IMemoryCache _memoryCache;

        public CachedUserRepository(IUserRepository innerRepository, IMemoryCache memoryCache)
        {
            _innerRepository = innerRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<User>> GetAllAsync(string? sortBy = null, bool ascending = true)
        {
            string cacheKey = "users";

            var users = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _innerRepository.GetAllAsync(sortBy, ascending);
            });

            return users!;
        }

        public async Task<User?> GetByUsernameAsync(Guid id, string username)
        {
            string cacheKey = $"username_{id}_{username.ToLower()}";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _innerRepository.GetByUsernameAsync(id, username);
            });
        }

        public async Task<User?> GetByEmailAsync(Guid id, string email)
        {
            string cacheKey = $"email_{id}_{email.ToLower()}";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _innerRepository.GetByEmailAsync(id, email);
            });
        }

        public async Task<User?> GetByIdAsync(Guid id)
            => await _innerRepository.GetByIdAsync(id);

        public Task AddAsync(User user)
            => _innerRepository.AddAsync(user);

        public void Update(User user)
            => _innerRepository.Update(user);

        public void Delete(User user)
            => _innerRepository.Delete(user);

        public async Task SaveChangesAsync()
            => await _innerRepository.SaveChangesAsync();
    }
}
