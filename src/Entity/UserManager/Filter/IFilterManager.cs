using System.Linq;

namespace Entity.UserManager.Filter
{
    public interface IFilterManager<TFilter, T>
    {
        IQueryable<T> Apply(TFilter filter, IQueryable<T> query, IDbContext dbContext);
    }
}
