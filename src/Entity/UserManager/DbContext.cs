using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tool;
using UserManager.Model;
using UserManager.Spi;

namespace Entity
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, 
        IUnitOfWork, 
        IUserRepository,
        IUserReadOnlyRepository,
        IAccountRepository
    {
        private readonly IHasher _hasher;
        private readonly IList<Action> _callbacks;

        public virtual DbSet<User> User { get; set; }

        IQueryable<IUpdateUserEntity> IUserRepository.Users => User.AsQueryable();
        IQueryable<IUserFull> IUserReadOnlyRepository.Users => User.AsQueryable();
        IQueryable<IUserLoginFilterable> IAccountRepository.Accounts => User.AsQueryable();

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

        public async Task<T> SaveChangesAsync<T>(T data)
        {
            await SaveChangesAsync();
            return data;
        }

        public void OnSaveChanges(Action callback)
        {
            _callbacks.Add(callback);
        }

        INewUserEntity IUserRepository.NewUser()
        {
            var entity = new User();
            User.Add(entity);
            return entity;
        }

        int IUserRepository.RemoveUser(int id)
        {
            User.Remove(User.Where(_ => _.Id == id).FirstOrDefault());
            return id;
        }
    }
}
