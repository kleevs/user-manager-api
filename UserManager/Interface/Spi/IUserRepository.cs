using System;
using System.Collections.Generic;
using System.Text;
using UserManager.Model;

namespace UserManager.Spi
{
    public interface IUserRepository
    {
        IEnumerable<IUser> List(IFilter filter);
        int Save(IUser user);
        void Delete(int id);
    }
}
