using Entity;
using Model;
using System.Collections.Generic;
using System.Linq;
using UserManager.Model;
using UserManager.Spi;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _domainDbContext;
        public UserRepository(IDbContext domainDbContext)
        {
            _domainDbContext = domainDbContext;
        }

        void IUserRepository.Delete(int id)
        {
            _domainDbContext.User.Remove(_domainDbContext.User.First(_ => _.Id == id));
            _domainDbContext.SaveChanges();
        }

        IEnumerable<IUser> IUserRepository.List(IFilter filter)
        {
            filter = filter ?? new Filter();
            return _domainDbContext.User
                .Where(_ => !filter.IsActive.HasValue || _.IsActive == filter.IsActive)
                .Where(_ => !filter.Id.HasValue || _.Id == filter.Id)
                .Where(_ => string.IsNullOrEmpty(filter.Email) || _.Email == filter.Email)
                .Where(_ => string.IsNullOrEmpty(filter.Password) || _.Password == filter.Password)
                .ToList();
        }

        int IUserRepository.Save(IUser user)
        {
            var entity = _domainDbContext.User.FirstOrDefault(_ => _.Id == user.Id);
            var parent = user.ParentUser != null ? _domainDbContext.User.FirstOrDefault(_ => _.Id == user.ParentUser.Id) : null;
            if (!user.Id.HasValue)
            {
                _domainDbContext.User.Add(entity = new User());
                entity.Password = user.Password;
                entity.Email = user.Email;
            }

            entity.LastName = user.LastName;
            entity.FirstName = user.FirstName;
            entity.IsActive = user.IsActive;
            entity.BirthDate = user.BirthDate;
            entity.ParentUser = parent;

            _domainDbContext.SaveChanges();
            return entity.Id.Value;
        }
    }
}
