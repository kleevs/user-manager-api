using System.Threading.Tasks;

namespace Entity
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<T> SaveChangesAsync<T>(T id);
    }
}
