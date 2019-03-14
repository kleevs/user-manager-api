using System;
using UserManager.Model;

namespace Web.Models
{
    public class UserOutputModel
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Id { get; set; }
        public int? ParentUser { get; set; }

        public static UserOutputModel Map(IUserData user) => user == null ? null : new UserOutputModel
        {
            Id = user.Id,
            Email = user.Email,
            LastName = user.LastName,
            FirstName = user.FirstName,
            IsActive = user.IsActive,
            BirthDate = user.BirthDate,
            ParentUser = user.ParentUser?.Id
        };
    }
}
