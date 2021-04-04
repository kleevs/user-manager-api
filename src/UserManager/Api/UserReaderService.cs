using System.Collections.Generic;
using System.Linq;
using UserManager.UserReaderServiceDeps;

namespace UserManager
{
    public class UserReaderService<TEntity> where TEntity : IUser
    {
        private readonly IUserReadOnlyRepository<TEntity> _userRepository;

        public UserReaderService(
            IUserReadOnlyRepository<TEntity> userRepository
        )
        {
            _userRepository = userRepository;
        }

        public IEnumerable<TEntity> List(IFilter filter)
        {
            var query = _userRepository.Users;

            if (filter != null) 
            {
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
                    query = query.Where(_ => _.Email.StartsWith(filter.Email));
                }
            }

            return query.OrderBy(_ => _.Id);
        }

        public IEnumerable<TEntity> List() => 
            List(null);
    }
}
