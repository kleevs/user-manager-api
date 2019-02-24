using System;
using UserManager.Model;

namespace Web.Models
{
    public class UserViewModel : IUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Id { get; set; }
        public int? ParentUser { get; set; }
        IUser IUser.ParentUser => new UserViewModel { Id = ParentUser };

        public static UserViewModel Map(IUser user) => user == null ? null : new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password,
            LastName = user.LastName,
            FirstName = user.FirstName,
            IsActive = user.IsActive,
            BirthDate = user.BirthDate,
            ParentUser = user.ParentUser?.Id
        };
    }
}
