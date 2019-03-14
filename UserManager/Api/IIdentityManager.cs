using UserManager.Model;

namespace UserManager
{
    public interface IIdentityManager
    {
        IUserData Login(string email, string password);
    }
}
