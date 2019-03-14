using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Entity
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync();
        ChangeTracker ChangeTracker { get; }

        DbSet<User> User { get; }
    }
}
