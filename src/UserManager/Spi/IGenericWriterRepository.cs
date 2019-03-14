namespace UserManager.Spi
{
    public interface IGenericWriterRepository<TInput>
    {
        int Save(TInput model);
    }

    public interface IGenericWriterRepository<TInput, TKey> : IGenericWriterRepository<TInput>
    {
        int Delete(TKey model);
    }
}
