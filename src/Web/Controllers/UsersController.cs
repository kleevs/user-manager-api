using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using UserManager;
using Web.Models;
using Web.Tools;

namespace Web.Controllers
{
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserReaderService _userReaderService;
        private readonly IUserWriterService _userWriterService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(
            IUserReaderService userReaderService, 
            IUserWriterService userWriterService,
            IUnitOfWork unitOfWork
        )
        {
            _userReaderService = userReaderService;
            _userWriterService = userWriterService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtient la liste des utilisateurs disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<UserOutputModel>> Index() =>
            await _userReaderService.List()
            .Select(UserOutputModel.Map)
            .ToListAsync();

        /// <summary>
        /// Obtient l'utilisateur ayant pour id {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOutputModel> Index(int id) => 
            await _userReaderService.List(new FilterInputModel { Id = id })
            .Select(UserOutputModel.Map)
            .FirstOrDefaultAsync();

        /// <summary>
        /// Définit l'utilisateur ayant pour id {id}
        /// </summary>
        /// <param name="model">Données de l'utilisateur</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Update([FromBody]UpdateUserInputModel model, int id)
        {
            model.Id = id;
            await _unitOfWork.SaveChangesAsync(_userWriterService.Save(model));
        }

        /// <summary>
        /// Définit un nouvel utilisateur
        /// </summary>
        /// <param name="model">Données de l'utilisateur</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Create([FromBody]NewUserInputModel model)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            model.ParentUser = userConnectedId;
            return (await _unitOfWork.SaveChangesAsync(_userWriterService.Save(model))).Id.Value;
        }

        /// <summary>
        /// Supprime l'utilisateur ayant pour id {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            await _unitOfWork.SaveChangesAsync(_userWriterService.Delete(id, userConnectedId));
        }
    }
}
