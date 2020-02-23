using System.Collections.Generic;
using System.Linq;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class UserReaderService : IUserReaderService
    {
        private readonly IUserReadOnlyRepository _userRepository;

        public UserReaderService(
            IUserReadOnlyRepository userRepository
        )
        {
            _userRepository = userRepository;
        }

        public IEnumerable<IUserData> List(IFilter filter)
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

        public IEnumerable<IUserData> List() => 
            List(null);
    }
}
