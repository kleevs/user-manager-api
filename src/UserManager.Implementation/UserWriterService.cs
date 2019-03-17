using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;

namespace UserManager.Implementation
{
    public class UserWriterService : IUserWriterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericWriterRepository<INewUser> _newUserRepository;
        private readonly IGenericWriterRepository<IUpdateUser, int> _updateUserRepository;

        public UserWriterService(
            IUnitOfWork unitOfWork,
            IGenericWriterRepository<INewUser> newUserRepository,
            IGenericWriterRepository<IUpdateUser, int> updateUserRepository)
        {
            _unitOfWork = unitOfWork;
            _newUserRepository = newUserRepository;
            _updateUserRepository = updateUserRepository;
        }

        public async Task Delete(int id, int userConnectedId) => 
            await _unitOfWork.SaveChangesAsync(_updateUserRepository.Delete(ControlDeleteUser(id, userConnectedId)));

        public async Task<int> Save(INewUser user) =>
            await _unitOfWork.SaveChangesAsync(_newUserRepository.Save(ControlNewUser(user)));

        public async Task<int> Save(IUpdateUser user) =>
            await _unitOfWork.SaveChangesAsync(_updateUserRepository.Save(ControlUser(user)));

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
