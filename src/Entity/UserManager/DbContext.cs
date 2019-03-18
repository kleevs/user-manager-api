using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tool;

namespace Entity
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        private readonly IHasher _hasher;

        public virtual DbSet<User> User { get; set; }

        public DbContext(DbContextOptions<DbContext> options, IHasher hasher) : base(options)
        {
            _hasher = hasher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(_ => _.Password)
                .HasConversion(
                    v => _hasher.Compute(v),
                    v => null
                );
        }

        public async Task<int> SaveChangesAsync() => 
            await base.SaveChangesAsync();
    }
}
