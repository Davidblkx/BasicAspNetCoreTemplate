using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwnAspNetCore.Services;

namespace OwnAspNetCore.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IDatabase _database;
        private ISecurity _security;

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}