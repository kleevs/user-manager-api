using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public IEnumerable<UserViewModel> Index()
        {
            return _userManager.List().Select(UserViewModel.Map);
        }

        [HttpGet]
        [Route("users/{id}")]
        public UserViewModel Index(int id)
        {
            return _userManager.List(new Filter { Id = id }).Select(UserViewModel.Map).First();
        }

        [HttpPut]
        [Route("users/{id}")]
        public void Update([FromBody]UserViewModel model, int id)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            model.ParentUser = userConnectedId;
            model.Id = id;
            _userManager.Save(model);
        }

        [HttpPost]
        [Route("users")]
        public void Create([FromBody]UserViewModel model)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            model.ParentUser = userConnectedId;
            model.Id = null;
            _userManager.Save(model);
        }

        [HttpDelete]
        [Route("users/{id}")]
        public void Delete(int id)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            _userManager.Delete(id, userConnectedId);
        }
    }
}
