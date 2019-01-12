using Model;
using UserManager.Model;

namespace Web.Models
{
    public class UserViewModel : User
    {
        public new int? ParentUser { get => base.ParentUser?.Id; set => base.ParentUser = new User { Id = value }; }
        public static new UserViewModel Map(IUser user) => user == null ? null : new UserViewModel
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
