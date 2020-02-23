using System;
using System.Collections.Generic;
using System.Linq;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class UserWriterService : IUserWriterService
    {
        private readonly IUserRepository _userRepository;

        public UserWriterService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Delete(int id, int userConnectedId) => 
            _userRepository.RemoveUser(ControlDeleteUser(id, userConnectedId));

        public INewUser Save(INewUser request)
        {
            request = ControlNewUser(request);
            var entity = _userRepository.NewUser();
            var parent = request.ParentUser != null ? _userRepository.Users.FirstOrDefault(_ => _.Id == request.ParentUser.Id) : null;

            entity.Password = request.Password;
            entity.Email = request.Email;
            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;
            entity.ParentUser = parent;

            return entity;
        }

        public IUpdateUser Save(IUpdateUser request)
        {
            request = ControlUser(request);

            var entity = _userRepository.Users.FirstOrDefault(_ => _.Id == request.Id);

            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;

            return entity;
        }

        private INewUser ControlNewUser(INewUser user)
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List<Action>
            {
                () => { if (string.IsNullOrEmpty(user.FirstName)) throw new FieldRequiredException(CodeError.FirstNameRequired, "first name"); },
                () => { if (string.IsNullOrEmpty(user.LastName)) throw new FieldRequiredException(CodeError.LastNameRequired, "last name"); },
                () => { if (!user.BirthDate.HasValue) throw new FieldRequiredException(CodeError.BirthDateRequired, "birth date"); },
                () => { if (string.IsNullOrEmpty(user.Email) && !user.Id.HasValue) throw new FieldRequiredException(CodeError.LoginRequired, "email"); },
                () => { if (string.IsNullOrEmpty(user.Password) && !user.Id.HasValue) throw new FieldRequiredException(CodeError.PasswordRequired, "password"); }
            });

            return user;
        }

        private IUpdateUser ControlUser(IUpdateUser user)
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
