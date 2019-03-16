using System.Linq;
using System.Threading.Tasks;
using UserManager.Implementation.Exception;
using UserManager.Implementation.Model;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IGenericReaderRepository<IFilter, IUserEmailable> _userRepository;
        private readonly IHasher _hasher;

        public IdentityManager(IGenericReaderRepository<IFilter, IUserEmailable> userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public IUserEmailable Login(string email, string password) => 
            _userRepository.List(new Filter
            {
                IsActive = true,
                Email = email,
                Password = _hasher.Compute(password)
            })
            .FirstOrDefault() ?? throw new LoginException();
    }
}
