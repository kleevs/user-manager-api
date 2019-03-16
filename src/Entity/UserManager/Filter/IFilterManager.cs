using System.Linq;

namespace Entity.Filter
{
    public interface IFilterManager<TFilter, T>
    {
        IQueryable<T> Apply(TFilter filter, IQueryable<T> query, IDbContext dbContext);
    }
}
