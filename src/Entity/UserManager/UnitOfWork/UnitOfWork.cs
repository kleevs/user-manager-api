using System.Linq;
using System.Threading.Tasks;
using UserManager.Spi;

namespace Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _domainDbContext;

        public UnitOfWork(IDbContext domainDbContext)
        {
            _domainDbContext = domainDbContext;
        }

        public async Task<int> SaveChangesAsync() => 
            await _domainDbContext.SaveChangesAsync();

        public async Task<T> SaveChangesAsync<T>(T data)
        {
            await SaveChangesAsync();
            return data;
        }
    }
}
