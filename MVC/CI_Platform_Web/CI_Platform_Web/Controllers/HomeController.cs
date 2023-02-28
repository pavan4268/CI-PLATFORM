using CI_Platform.Entities;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.Data.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;

namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {

        //private readonly ILoginRepository _loginuser;
        private readonly IRegistrationRepository _registration;
        private readonly CiPlatformDbContext _db;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(/*ILoginRepository loginuser, */IRegistrationRepository registration, CiPlatformDbContext db)
        {
            //_loginuser = loginuser;
            _registration = registration;
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult forgot()
        {
            return View();
        }

        public IActionResult registration()
        {
            return View();
        }

        public IActionResult reset()
        {
            return RedirectToAction("Index");
        }

        // Login 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginVm obj)
        {
            if (ModelState.IsValid)
            {
                if(_db!=null)
                {
                    var a = obj.Email;
                    var b = obj.Password;
                    var issuccess = _db.Users.FirstOrDefault(c => c.Email == a && c.Password == b);
                    if (issuccess != null)
                    {
                        //TempData["success"] = "login successful";
                        return RedirectToAction("PlatformLanding", "Mission");
                    }
                    else
                    {
                        TempData["alert"] = "incorrect credentials";
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            return View();
        }
        //registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult registration(User obj)
        {
            if (ModelState.IsValid)
            {
                _registration.Users.Add(obj);
            }
            return View();
        }
        //registration
    }
}
//else
//                {
//                    TempData["alert"] = "data is null";
//                    return RedirectToAction("login", "home");
//    }
//}
//return View();

//if (issucess != null)
//            {
//                return RedirectToAction("PlatformLanding", "Mission");
//            }
//            else
//            {
//                return RedirectToAction("Index", "Home");
//            }

// login




//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    public IActionResult Error()
//    {
//        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    }
//}
