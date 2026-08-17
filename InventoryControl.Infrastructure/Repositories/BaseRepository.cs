using InventoryControl.Domain.Interfaces.Repositories;
using InventoryControl.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace InventoryControl.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? sortBy = null, bool ascending = true)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var direction = ascending ? "ascending" : "descending";
                    query = query.OrderBy($"{sortBy} {direction}");
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public Task AddAsync(T entity)
            => _dbSet.AddAsync(entity).AsTask();

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
