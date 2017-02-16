using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwnAspNetCore.ApiModels;
using OwnAspNetCore.Models;
using OwnAspNetCore.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using System;

namespace OwnAspNetCore.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IDatabase _database;
        private ISecurity _security;

        public AuthController(IDatabase db, ISecurity s)
        {
            _database = db;
            _security = s;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(ModelUserLogin login)
        {
            login.Validate(ModelState);

            if (!ModelState.IsValid) return View();

            // ------------ User Validation -----------------
            IUser user = _database.Search<User>(x => x.Username == login.User).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("User", "Username is invalid");
                return View();
            }

            if (!_security.Validate(user.Hash, login.Pass, user.Salt))
            {
                ModelState.AddModelError("Pass", "Password is invalid");
                return View();
            }
            // ------------ End Validation -----------------

            string claimIssuer = this.GetType().Namespace;
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String, claimIssuer));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer, claimIssuer));
            foreach (string role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, claimIssuer));

            var userIdentity = new ClaimsIdentity("CookieAuth");
            userIdentity.AddClaims(claims);

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authProperties = new AuthenticationProperties();
            authProperties.AllowRefresh = false;
            authProperties.ExpiresUtc = DateTime.UtcNow.AddHours(8);
            authProperties.IsPersistent = true;

            await HttpContext.Authentication.SignInAsync(
                    "CookieAuth",
                    userPrincipal,
                    authProperties
                );

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/Forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}