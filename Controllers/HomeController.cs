using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwnAspNetCore.Infra;

namespace OwnAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Admin")]
        [Authorize(Policy = UserRoles.Admin)]
        public IActionResult Admin()
        {
            return View();
        }
    }
}