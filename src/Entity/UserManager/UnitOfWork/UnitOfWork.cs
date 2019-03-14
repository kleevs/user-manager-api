using Entity;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Spi;

namespace UnitOfWork
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

        public async Task<int> SaveChangesAsync(int id)
        {
            var property = _domainDbContext.ChangeTracker.Entries()
                .SelectMany(_ =>_.Properties)
                .Where(_ => _.CurrentValue is int)
                .Where(_ => _.IsTemporary)
                .Where(_ => (int)_.CurrentValue == id)
                .FirstOrDefault();

            await SaveChangesAsync();
            return property?.OriginalValue as int? ?? id;
        }
    }
}
