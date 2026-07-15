using InventoryControl.Domain.Interfaces;
using InventoryControl.Infrastructure.Context;

namespace InventoryControl.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
            => _context = context;

        public async Task<int> CommitAsync()
            => await _context.SaveChangesAsync();
    }
}
