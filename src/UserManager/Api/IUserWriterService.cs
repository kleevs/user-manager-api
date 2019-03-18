using UserManager.Model;

namespace UserManager
{
    public interface IUserWriterService
    {
        int Save(INewUser user);
        int Save(IUpdateUser user);
        int Delete(int user, int userConnectedId);
    }
}
