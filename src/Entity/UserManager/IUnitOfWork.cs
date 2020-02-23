using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Entity
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<T> SaveChangesAsync<T>(T data);
    }
}
