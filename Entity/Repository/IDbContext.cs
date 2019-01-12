using Microsoft.EntityFrameworkCore;
using Model;

namespace Entity
{
    public interface IDbContext
    {
        int SaveChanges();
        DbSet<User> User { get; }
    }
}
