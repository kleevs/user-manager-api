namespace UserManager.Spi
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        int SaveChanges(int id);
    }
}
