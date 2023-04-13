using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminHomePage()
        {
            return View();
        }
    }
}
