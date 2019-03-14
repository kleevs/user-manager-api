using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManager.Model;

namespace Entity
{
    [Table("User", Schema = "UserManager")]
    public class User : IUser, INewUser, IUserData
    {
        [Key]
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public User ParentUser { get; set; }

        IUser IHerarchy<IUser>.ParentUser => ParentUser;
    }
}
