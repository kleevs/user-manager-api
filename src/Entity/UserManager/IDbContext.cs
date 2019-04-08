using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Entity
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync();
        void OnSaveChanges(System.Action callback);
        //ChangeTracker ChangeTracker { get; }

        DbSet<User> User { get; }
    }
}
