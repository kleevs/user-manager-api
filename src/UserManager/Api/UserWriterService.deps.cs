using System;
using System.Linq;

namespace UserManager.UserWriterServiceDeps
{
    public interface IUserWriterRepository<TUserEntity> where TUserEntity : UserEntity
    {
        IQueryable<TUserEntity> Users { get; }
        TUserEntity NewUser();
        int RemoveUser(int id);
    }

    public interface IUserData 
    {
        string LastName { get; }
        string FirstName { get; }
        bool IsActive { get; }
        DateTime? BirthDate { get; }
    }

    public interface INewUser : IUserData
    {
        int? ParentUserId { get; set; }
        string Email { get; }
        string Password { get; }
    }
    
    public interface IUpdateUser : IUserData
    {
        int Id { get; }
    }

    public interface UserEntity : IUserData
    {
        int? Id { get; }
        string Email { get; set; }
        string Password { get; set; }
        new string LastName { get; set; }
        new string FirstName { get; set; }
        new bool IsActive { get; set; }
        new DateTime? BirthDate { get; set; }
        UserEntity ParentUser { get; set; }
    }
}
