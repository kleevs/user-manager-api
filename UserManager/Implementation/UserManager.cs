using System;
using System.Collections.Generic;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;

        public UserManager(IUnitOfWork unitOfWork, IUserRepository userRepository, IHasher hasher)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _hasher = hasher;
        }

        void IUserManager.Delete(int id, int userConnectedId)
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List<Action>
            {
                () => { if (id == userConnectedId) throw new BusinessException(CodeError.DeleteUserConnected); }
            });
            _userRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }

        IEnumerable<IUser> IUserManager.List(IFilter filter)
        {
            return _userRepository.List(filter);
        }

        IEnumerable<IUser> IUserManager.List() => ((IUserManager)this).List(null);

        int IUserManager.Save(IUser user)
        {
            ArrayException.Assert<BusinessException>(CodeError.FieldRequired, new List <Action>
            {
                () => { if (string.IsNullOrEmpty(user.FirstName)) throw new FieldRequiredException(CodeError.FirstNameRequired, "first name"); },
                () => { if (string.IsNullOrEmpty(user.LastName)) throw new FieldRequiredException(CodeError.LastNameRequired, "last name"); },
                () => { if (!user.BirthDate.HasValue) throw new FieldRequiredException(CodeError.BirthDateRequired, "birth date"); },
                () => { if (string.IsNullOrEmpty(user.Email) && !user.Id.HasValue) throw new FieldRequiredException(CodeError.LoginRequired, "email"); },
                () => { if (string.IsNullOrEmpty(user.Password) && !user.Id.HasValue) throw new FieldRequiredException(CodeError.PasswordRequired, "password"); }
            });

            user.Password = _hasher.Compute(user.Password);
            var id = _userRepository.Save(user);
            return _unitOfWork.SaveChanges(id);
        }
    }
}
