using Entity;
using Entity.UserManager.Filter;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UserManager.Implementation.Model;
using UserManager.Model;
using UserManager.Spi;

namespace Repository
{
    public class UserRepository :
        IGenericReaderRepository<IFilter, IUserData>,
        IGenericWriterRepository<INewUser>,
        IGenericWriterRepository<IUpdateUser, int>
    {
        private readonly IDbContext _domainDbContext;
        private readonly IFilterManager<IFilter, User> _filterManager;

        public UserRepository(IDbContext domainDbContext, IFilterManager<IFilter, User> filterManager)
        {
            _domainDbContext = domainDbContext;
            _filterManager = filterManager;
        }

        public int Delete(int id)
        {
            _domainDbContext.User.Remove(_domainDbContext.User.First(_ => _.Id == id));
            return 0;
        }

        public int Delete(INewUser model) =>
            Delete(model.Id.Value);

        public int Delete(IUpdateUser model) =>
            Delete(model.Id.Value);

        public IEnumerable<IUserData> List(IFilter filter) => 
            _filterManager.Apply(filter, _domainDbContext.User
                .Include(_ => _.ParentUser)
                .AsNoTracking(), _domainDbContext);

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
