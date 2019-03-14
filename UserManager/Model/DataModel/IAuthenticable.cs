namespace UserManager.Model
{
    public interface IAuthenticable : IEmailable
    {
        string Password { get; }
    }
}
