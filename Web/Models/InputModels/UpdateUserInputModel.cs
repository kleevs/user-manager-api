using System;
using UserManager.Model;

namespace Web.Models
{
    public class UpdateUserInputModel : IUpdateUser
    {
        public int? Id { get; set; }
        public DateTime? BirthDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
    }
}
