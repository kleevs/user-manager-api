using System.Linq;

namespace UserManager
{
    public interface IFilterManager<TFilter, T>
    {
        IQueryable<T> Apply(TFilter filter, IQueryable<T> query);
    }
}
