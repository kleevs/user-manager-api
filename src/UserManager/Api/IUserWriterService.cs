using System.Threading.Tasks;
using UserManager.Model;

namespace UserManager
{
    public interface IUserWriterService
    {
        Task<int> Save(INewUser user);
        Task<int> Save(IUpdateUser user);
        Task Delete(int user, int userConnectedId);
    }
}
