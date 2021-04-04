using System;
using UserManager.UserWriterServiceDeps;

namespace Web.Models
{
    public class NewUserInputModel : INewUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Id { get; set; }
        public int? ParentUserId { get; set; }
    }
}
