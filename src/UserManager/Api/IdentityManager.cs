using System.Linq;
using UserManager.Implementation.Exception;
using UserManager.IdentityManagerDeps;

namespace UserManager
{
    public class IdentityManager<TEntity> where TEntity : IUserEntity
    {
        private readonly IAccountRepository<TEntity> _userRepository;

        public IdentityManager(
            IAccountRepository<TEntity> userRepository
        )
        {
            _userRepository = userRepository;
        }

        public TEntity Login(string email, string password)
        {
            var result = _userRepository.Accounts
                .Where(_ => _.Email == email)
                .Where(_ => _.Password == password)
                .FirstOrDefault();

            if (result == null)
            {
                throw new LoginException();
            }

            return result;
        }
    }
}
