using System;

namespace UserManager.Model
{
    public interface INewUser : IUser, IBirthData, INewAuthenticable, INameable, IActivable, IHerarchy<IUser>
    {
    }

    public interface INewUserEntity : INewUser
    {
        new string Email { get; set; }
        new string Password { get; set; }
        new string LastName { get; set; }
        new string FirstName { get; set; }
        new bool IsActive { get; set; }
        new DateTime? BirthDate { get; set; }
        new IUser ParentUser { get; set; }
    }
}
