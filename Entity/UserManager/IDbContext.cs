using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Entity
{
    public interface IDbContext
    {
        int SaveChanges();
        ChangeTracker ChangeTracker { get; }

        DbSet<User> User { get; }
    }
}
