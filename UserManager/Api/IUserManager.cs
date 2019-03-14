using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.Model;

namespace UserManager
{
    public interface IUserManager
    {
        IEnumerable<IUserData> List();
        IEnumerable<IUserData> List(IFilter filter);
        Task<int> Save(INewUser user);
        Task<int> Save(IUpdateUser user);
        Task Delete(int user, int userConnectedId);
    }
}
