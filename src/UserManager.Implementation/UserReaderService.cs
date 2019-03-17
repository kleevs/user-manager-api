using System.Collections.Generic;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class UserReaderService : IUserReaderService
    {
        private readonly IGenericReaderRepository<IUserFilterable> _userRepository;
        private readonly IFilterManager<IFilter, IUserFilterable> _userFilterManager;

        public UserReaderService(
            IGenericReaderRepository<IUserFilterable> userRepository,
            IFilterManager<IFilter, IUserFilterable> userFilterManager
        )
        {
            _userRepository = userRepository;
            _userFilterManager = userFilterManager;
        }

        public IEnumerable<IUserData> List(IFilter filter) =>
            _userFilterManager.Apply(filter, _userRepository.List());

        public IEnumerable<IUserData> List() => 
            List(null);
    }
}
