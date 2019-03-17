using System.Collections.Generic;
using UserManager.Model;

namespace UserManager
{
    public interface IUserReaderService
    {
        IEnumerable<IUserData> List();
        IEnumerable<IUserData> List(IFilter filter);
    }
}
