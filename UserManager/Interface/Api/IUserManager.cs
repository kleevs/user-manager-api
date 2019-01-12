using System.Collections.Generic;
using UserManager.Model;

namespace UserManager
{
    public interface IUserManager
    {
        IEnumerable<IUser> List();
        IEnumerable<IUser> List(IFilter filter);
        int Save(IUser user);
        void Delete(int id, int userConnectedId);
    }
}
