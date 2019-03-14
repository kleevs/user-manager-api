namespace UserManager.Model
{
    public interface INewAuthenticable : IEmailable
    {
        string Password { get; set; }
    }
}
