using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using UserManager;
using Web.Models;

namespace Web.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<UserOutputModel>> Index() =>
            await _userManager.List()
            .Select(UserOutputModel.Map)
            .ToAsyncEnumerable()
            .ToList();

        [HttpGet]
        [Route("users/{id}")]
        public async Task<UserOutputModel> Index(int id) => 
            await _userManager.List(new FilterInputModel { Id = id })
            .Select(UserOutputModel.Map)
            .ToAsyncEnumerable()
            .First();

        [HttpPut]
        [Route("users/{id}")]
        public async Task Update([FromBody]UpdateUserInputModel model, int id)
        {
            model.Id = id;
            await _userManager.Save(model);
        }

        [HttpPost]
        [Route("users")]
        public async Task Create([FromBody]NewUserInputModel model)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            model.ParentUser = userConnectedId;
            await _userManager.Save(model);
        }

        [HttpDelete]
        [Route("users/{id}")]
        public async Task Delete(int id)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            await _userManager.Delete(id, userConnectedId);
        }
    }
}
