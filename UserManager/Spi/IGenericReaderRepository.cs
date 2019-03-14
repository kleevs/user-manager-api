using System.Collections.Generic;

namespace UserManager.Spi
{
    public interface IGenericReaderRepository<TFilter, TOutput>
    {
        IEnumerable<TOutput> List(TFilter filter);
    }

    public interface IGenericReaderRepository<TOutput>
    {
        IEnumerable<TOutput> List();
    }
}
