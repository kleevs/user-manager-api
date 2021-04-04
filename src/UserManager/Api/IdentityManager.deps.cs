using System.Linq;

namespace UserManager.IdentityManagerDeps
{
    public interface IAccountRepository<T> where T : IUserEntity
    {
        IQueryable<T> Accounts { get; }
    }

    public interface IUserEntity
    {
        string Email { get; }
        string Password { get; }
    }
}
