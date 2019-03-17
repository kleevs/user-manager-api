using System.Linq;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class LoginFilterManager : IFilterManager<ILoginFilter, IUserLoginFilterable>
    {
        private readonly IHasher _hasher;

        public LoginFilterManager(IHasher hasher)
        {
            _hasher = hasher;
        }

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
                query = query.Where(_ => _.Password == _hasher.Compute(filter.Password));
            }

            return query.OrderBy(_ => _.Id);
        }
    }
}
