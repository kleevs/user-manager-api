using System.Linq;

namespace UserManager.Spi
{
    public interface IGenericReaderRepository<TOutput>
    {
        IQueryable<TOutput> List();
    }
}
