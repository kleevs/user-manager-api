using System;

namespace UserManager.Model
{
    public interface IUpdateUser : IUser, IBirthData, INameable, IActivable
    {
    }

    public interface IUpdateUserEntity : IUpdateUser
    {
        new string LastName { get; set; }
        new string FirstName { get; set; }
        new bool IsActive { get; set; }
        new DateTime? BirthDate { get; set; }
    }
}
