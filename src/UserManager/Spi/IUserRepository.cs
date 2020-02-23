using System.Linq;
using UserManager.Model;

namespace UserManager.Spi
{
    public interface IUserRepository
    {
        IQueryable<IUpdateUserEntity> Users { get; }
        INewUserEntity NewUser();
        int RemoveUser(int id);
    }

    public interface IAccountRepository 
    {
        IQueryable<IUserLoginFilterable> Accounts { get; }
    }

    public interface IUserReadOnlyRepository
    {
        IQueryable<IUserFull> Users { get; }
    }
}
