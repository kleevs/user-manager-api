using UserManager.Model;

namespace UserManager
{
    public interface IIdentityManager
    {
        IUserEmailable Login(string email, string password);
    }
}
