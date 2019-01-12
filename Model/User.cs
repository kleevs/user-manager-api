using System;
using UserManager.Model;

namespace Model
{
    public class User : IUser
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public User ParentUser { get; set; }
        IUser IUser.ParentUser { get => ParentUser; set => ParentUser = ParentUser as User ?? Map(value); }

        public static User Map(IUser user) => user == null ? null : new User
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password,
            LastName = user.LastName,
            FirstName = user.FirstName,
            IsActive = user.IsActive,
            BirthDate = user.BirthDate,
            ParentUser = Map(user.ParentUser)
        };
    }
}
