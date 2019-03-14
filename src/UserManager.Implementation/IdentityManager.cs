using System.Linq;
using UserManager.Implementation.Exception;
using UserManager.Implementation.Model;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IGenericReaderRepository<IFilter, IUserData> _userRepository;
        private readonly IHasher _hasher;

        public IdentityManager(IGenericReaderRepository<IFilter, IUserData> userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public IUserData Login(string email, string password) => 
            _userRepository.List(new Filter
            {
                IsActive = true,
                Email = email,
                Password = _hasher.Compute(password)
            })
            .FirstOrDefault() ?? throw new LoginException();
    }
}
