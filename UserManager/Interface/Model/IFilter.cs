namespace UserManager.Model
{
    public interface IFilter
    {
        int? Id { get; }
        string Email { get; }
        string Password { get; }
        bool? IsActive { get; }
    }
}
