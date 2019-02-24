using Entity;
using System.Linq;
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

        public int SaveChanges() => _domainDbContext.SaveChanges();

        public int SaveChanges(int id)
        {
            var property = _domainDbContext.ChangeTracker.Entries()
                .SelectMany(_ =>_.Properties)
                .Where(_ => _.CurrentValue is int)
                .Where(_ => _.IsTemporary)
                .Where(_ => (int)_.CurrentValue == id)
                .FirstOrDefault();

            SaveChanges();
            return property?.OriginalValue as int? ?? id;
        }
    }
}
