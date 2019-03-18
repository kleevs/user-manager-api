using System.Linq;
using UserManager.Model;

namespace UserManager.Implementation
{
    public class LoginFilterManager : IFilterManager<ILoginFilter, IUserLoginFilterable>
    {
        public IQueryable<IUserLoginFilterable> Apply(ILoginFilter filter, IQueryable<IUserLoginFilterable> query)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(_ => _.Email == filter.Email);
            }

            if (!string.IsNullOrEmpty(filter.Password))
            {
                query = query.Where(_ => _.Password == filter.Password);
            }

            return query.OrderBy(_ => _.Id);
        }
    }
}
