using Microsoft.AspNetCore.Mvc;

namespace OwnAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}