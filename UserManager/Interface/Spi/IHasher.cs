namespace UserManager.Spi
{
    public interface IHasher
    {
        string Compute(string text);
    }
}
