using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UserManager;

namespace Web.Controllers
{
    public class AccountsController : ControllerBase
    {
        private readonly IIdentityManager _identityManager;
        public readonly ILogger<AccountsController> _logger;

        public AccountsController(IIdentityManager identityManager, ILogger<AccountsController> logger)
        {
            _identityManager = identityManager;
            _logger = logger;
        }

        [HttpGet]
        public object Index()
        {
            var userConnectedId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
            return new { id = userConnectedId };
        }

        [HttpGet]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task Login(string login, string password)
        {
            var user = _identityManager.Login(login, password);
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

            _logger.LogInformation("Connexion réussi : {USER}", login);
        }
    }
}
