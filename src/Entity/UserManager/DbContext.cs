using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public virtual DbSet<User> User { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public async Task<int> SaveChangesAsync() => 
            await base.SaveChangesAsync();
    }
}
