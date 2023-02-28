using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class MissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PlatformLanding()
        {
            return View();
        }
    }
}
