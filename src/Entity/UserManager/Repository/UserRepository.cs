using Microsoft.EntityFrameworkCore;
using System.Linq;
using UserManager.Model;
using UserManager.Spi;

namespace Entity.Repository
{
    public class UserRepository :
        IGenericReaderRepository<IUserFilterable>,
        IGenericReaderRepository<IUserEmailable>,
        IGenericWriterRepository<INewUser>,
        IGenericWriterRepository<IUpdateUser, int>,
        IGenericReaderRepository<IUserLoginFilterable>
    {
        private readonly IDbContext _domainDbContext;

        public UserRepository(IDbContext domainDbContext)
        {
            _domainDbContext = domainDbContext;
        }

        public int Delete(int id)
        {
            _domainDbContext.User.Remove(_domainDbContext.User.First(_ => _.Id == id));
            return 0;
        }

        public IQueryable<User> List() =>
            _domainDbContext.User
                .Include(_ => _.ParentUser)
                .AsNoTracking();

        IQueryable<IUserFilterable> IGenericReaderRepository<IUserFilterable>.List() => List();
        IQueryable<IUserEmailable> IGenericReaderRepository<IUserEmailable>.List() => List();
        IQueryable<IUserLoginFilterable> IGenericReaderRepository<IUserLoginFilterable>.List() => List();

        public int Save(INewUser user)
        {
            var entity = new User();
            var parent = user.ParentUser != null ? _domainDbContext.User.FirstOrDefault(_ => _.Id == user.ParentUser.Id) : null;

            _domainDbContext.User.Add(entity);

            entity.Password = user.Password;
            entity.Email = user.Email;
            entity.LastName = user.LastName;
            entity.FirstName = user.FirstName;
            entity.IsActive = user.IsActive;
            entity.BirthDate = user.BirthDate;
            entity.ParentUser = parent;

            return entity.Id.Value;
        }

        public int Save(IUpdateUser user)
        {
            var entity = _domainDbContext.User.FirstOrDefault(_ => _.Id == user.Id);

            entity.LastName = user.LastName;
            entity.FirstName = user.FirstName;
            entity.IsActive = user.IsActive;
            entity.BirthDate = user.BirthDate;

            return entity.Id.Value;
        }
    }
}
