using System;

namespace UserManager.Model
{
    public interface IUser : IIdentifiable
    {
        string Email { get; set; }
        string Password { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        bool IsActive { get; set; }
        DateTime? BirthDate { get; set; }
        IUser ParentUser { get; set; }
    }
}
