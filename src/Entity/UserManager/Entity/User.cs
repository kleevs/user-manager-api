using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IReaderUser = UserManager.UserReaderServiceDeps.IUser;
using IWriterUserEntity = UserManager.UserWriterServiceDeps.UserEntity;

namespace Entity
{
    [Table("User", Schema = "UserManager")]
    public class User :
        UserManager.IdentityManagerDeps.IUserEntity,
        IWriterUserEntity,
        IReaderUser
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
        IWriterUserEntity IWriterUserEntity.ParentUser 
        { 
            get => ParentUser; 
            set => ParentUser = value as User ?? (value != null ? new User { Id = value.Id } : null); 
        }

        int IReaderUser.Id 
        {
            get => ParentUser.Id.Value;
        }
    }
}
