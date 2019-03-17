using System.Linq;
using UserManager.Implementation.Exception;
using UserManager.Implementation.Model;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IGenericReaderRepository<IUserLoginFilterable> _userRepository;
        private readonly IFilterManager<ILoginFilter, IUserLoginFilterable> _userFilterManager;

        public IdentityManager(
            IGenericReaderRepository<IUserLoginFilterable> userRepository, 
            IFilterManager<ILoginFilter, IUserLoginFilterable> userFilterManager
        )
        {
            _userRepository = userRepository;
            _userFilterManager = userFilterManager;
        }

        public IUserEmailable Login(string email, string password) =>
            _userFilterManager.Apply(new LoginFilter
            {
                Email = email,
                Password = password
            }, _userRepository.List())
            .FirstOrDefault() ?? throw new LoginException();
    }
}
