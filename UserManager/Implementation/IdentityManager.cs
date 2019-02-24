using System.Linq;
using UserManager.Implementation.Exception;
using UserManager.Implementation.Model;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;

        public IdentityManager(IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        IUser IIdentityManager.Login(string email, string password) => _userRepository.List(new Filter { IsActive = true, Email = email, Password = _hasher.Compute(password) }).FirstOrDefault() ?? throw new LoginException();
    }
}
