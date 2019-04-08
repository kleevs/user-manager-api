using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tool;

namespace Entity
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        private readonly IHasher _hasher;
        private readonly IList<Action> _callbacks;

        public virtual DbSet<User> User { get; set; }

        public DbContext(DbContextOptions<DbContext> options, IHasher hasher) : base(options)
        {
            _hasher = hasher;
            _callbacks = new List<Action>();
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

        public async Task<int> SaveChangesAsync()
        {
            var res = await base.SaveChangesAsync();
            foreach (var callback in _callbacks) callback();
            return res;
        }

        public void OnSaveChanges(Action callback)
        {
            _callbacks.Add(callback);
        }
    }
}
