using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UserManager;
using Web.Models;

namespace Web.Controllers
{
    public class AccountsController : ControllerBase
    {
        private readonly IIdentityManager _identityManager;

        public AccountsController(IIdentityManager identityManager)
        {
            _identityManager = identityManager;
        }

        [HttpGet]
        public object Index()
        {
            var userConnectedId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
            return new { id = userConnectedId };
        }

        [HttpPost]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task Login([FromBody]LoginViewModel form)
        {
            var user = _identityManager.Login(form.Login, form.Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
