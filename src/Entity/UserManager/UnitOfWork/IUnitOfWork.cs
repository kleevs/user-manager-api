using System.Threading.Tasks;

namespace Entity
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(int id);
    }
}
