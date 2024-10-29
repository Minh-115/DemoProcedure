using WebApplication2.Models;
using WebApplication2.Repositories.Repositories_Base;

namespace WebApplication2.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        Task SaveAsync();
    }
}
