namespace UserManager.Spi
{
    public interface IGenericWriterRepository<TInput>
    {
        TInput Save(TInput model);
    }

    public interface IGenericWriterRepository<TInput, TKey> : IGenericWriterRepository<TInput>
    {
        int Delete(TKey model);
    }
}
