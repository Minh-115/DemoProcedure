using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repositories.Repositories_Base;

namespace WebApplication2.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<Product> _products;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
