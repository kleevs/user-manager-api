using System;

namespace UserManager.Model
{
    public interface IUser : IIdentifiable
    {
        string Email { get; }
        string Password { get; set; }
        string LastName { get; }
        string FirstName { get; }
        bool IsActive { get; }
        DateTime? BirthDate { get; }
        IUser ParentUser { get; }
    }
}
