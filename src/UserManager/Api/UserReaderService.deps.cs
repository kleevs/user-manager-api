using System.Linq;

namespace UserManager.UserReaderServiceDeps
{
    public interface IUserReadOnlyRepository<T> where T : IUser
    {
        IQueryable<T> Users { get; }
    }

    public interface IUser
    {
        int Id { get; }
        string Email { get; }
        bool IsActive { get; }
    }

    public interface IFilter
    {
        int? Id { get; }
        string Email { get; }
        bool? IsActive { get; }
    }
}
