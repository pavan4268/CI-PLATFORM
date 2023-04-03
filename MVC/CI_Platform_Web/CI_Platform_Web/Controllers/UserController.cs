using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserProfile()
        {
            return View();
        }
    }
}
