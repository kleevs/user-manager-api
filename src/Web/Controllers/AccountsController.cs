﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManager;
using Web.Models;

namespace Web.Controllers
{
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IIdentityManager _identityManager;

        public AccountsController(IIdentityManager identityManager)
        {
            _identityManager = identityManager;
        }

        /// <summary>
        /// Obtient les informations sur l'utilisateur connecté
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Index()
        {
            var userConnectedId = await HttpContext.User.Claims
                .ToAsyncEnumerable()
                .Where(_ => _.Type == ClaimTypes.NameIdentifier)
                .Select(_ => _.Value)
                .FirstOrDefault();

            return new { id = userConnectedId };
        }

        /// <summary>
        /// Connexion d'un utilisateur
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task Login([FromBody]LoginInputModel form)
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
