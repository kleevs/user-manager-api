using System;
using System.Collections.Generic;
using System.Linq;
using UserManager.Constant;
using UserManager.Implementation.Exception;
using UserManager.UserWriterServiceDeps;

namespace UserManager
{
    public class UserWriterService<TEntity> where TEntity : UserEntity
    {
        private readonly IUserWriterRepository<TEntity> _userRepository;

        public UserWriterService(
            IUserWriterRepository<TEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public int Delete(int id, int userConnectedId) => 
            _userRepository.RemoveUser(ControlDeleteUser(id, userConnectedId));

        public TEntity Save(INewUser request)
        {
            request = ControlNewUser(request);
            var entity = _userRepository.NewUser();
            var parent = request.ParentUserId != null ? _userRepository.Users.FirstOrDefault(_ => _.Id == request.ParentUserId) : default(TEntity);

            entity.Password = request.Password;
            entity.Email = request.Email;
            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;
            entity.ParentUser = parent;

            return entity;
        }

        public TEntity Save(IUpdateUser request)
        {
            request = ControlUser(request);

            var entity = _userRepository.Users.FirstOrDefault(_ => _.Id == request.Id);

            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;

            return entity;
        }

        private T ControlNewUser<T>(T user) where T : INewUser
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List<Action>
            {
                () => { if (string.IsNullOrEmpty(user.FirstName)) throw new FieldRequiredException(CodeError.FirstNameRequired, "first name"); },
                () => { if (string.IsNullOrEmpty(user.LastName)) throw new FieldRequiredException(CodeError.LastNameRequired, "last name"); },
                () => { if (!user.BirthDate.HasValue) throw new FieldRequiredException(CodeError.BirthDateRequired, "birth date"); },
                () => { if (string.IsNullOrEmpty(user.Email)) throw new FieldRequiredException(CodeError.LoginRequired, "email"); },
                () => { if (string.IsNullOrEmpty(user.Password)) throw new FieldRequiredException(CodeError.PasswordRequired, "password"); }
            });

            return user;
        }

        private T ControlUser<T>(T user) where T : IUserData
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List<Action>
            {
                () => { if (string.IsNullOrEmpty(user.FirstName)) throw new FieldRequiredException(CodeError.FirstNameRequired, "first name"); },
                () => { if (string.IsNullOrEmpty(user.LastName)) throw new FieldRequiredException(CodeError.LastNameRequired, "last name"); },
                () => { if (!user.BirthDate.HasValue) throw new FieldRequiredException(CodeError.BirthDateRequired, "birth date"); },
            });

            return user;
        }

        private int ControlDeleteUser(int id, int userConnectedId)
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List<Action>
            {
                () => { if (id == userConnectedId) throw new BusinessException(CodeError.DeleteUserConnected); }
            });

            return id;
        }
    }
}
