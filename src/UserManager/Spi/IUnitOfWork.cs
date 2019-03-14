using System.Threading.Tasks;

namespace UserManager.Spi
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(int id);
    }
}
