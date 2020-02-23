using System.Linq;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IAccountRepository _userRepository;

        public IdentityManager(
            IAccountRepository userRepository
        )
        {
            _userRepository = userRepository;
        }

        public IUserEmailable Login(string email, string password) =>
            _userRepository.Accounts
                .Where(_ => _.Email == email)
                .Where(_ => _.Password == password)
                .FirstOrDefault() ?? throw new LoginException();
    }
}
