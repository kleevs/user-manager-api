using UserManager.Model;

namespace UserManager
{
    public interface IUserWriterService
    {
        INewUser Save(INewUser user);
        IUpdateUser Save(IUpdateUser user);
        int Delete(int user, int userConnectedId);
    }
}
