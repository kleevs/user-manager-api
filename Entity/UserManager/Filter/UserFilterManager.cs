using System.Linq;
using UserManager.Model;

namespace Entity.UserManager.Filter
{
    public class UserFilterManager : IFilterManager<IFilter, User>
    {
        public IQueryable<User> Apply(IFilter filter, IQueryable<User> query, IDbContext dbContext)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(_ => _.IsActive == filter.IsActive);
            }

            if (filter.Id.HasValue)
            {
                query = query.Where(_ => _.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(_ => _.Email == filter.Email);
            }

            if (!string.IsNullOrEmpty(filter.Password))
            {
                query = query.Where(_ => _.Password == filter.Password);
            }

            return query;
        }
    }
}
