using UserManager.Model;

namespace UserManager
{
    public interface IIdentityManager
    {
        IUser Login(string email, string password);
    }
}
