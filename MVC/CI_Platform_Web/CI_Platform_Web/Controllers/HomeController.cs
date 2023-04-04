using CI_Platform.Entities;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;
using System.Diagnostics;
using System.Net.Mail;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;


namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {

        //private readonly ILoginRepository _loginuser;
        //private readonly IRegistrationRepository _registration;
        private readonly CiPlatformDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(/*ILoginRepository loginuser, *//*IRegistrationRepository registration,*/ CiPlatformDbContext db, IHttpContextAccessor contextAccessor)
        {
            //_loginuser = loginuser;
            //_registration = registration;
            _db = db;
            _contextAccessor = contextAccessor;
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
            return View();
        }


        //logout

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("FirstName");
            return RedirectToAction("Index");
        }

        //logout

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
                    //var firstname = obj.FirstName;
                    //var lastname = obj.LastName;    
                    //var name = firstname + " " + lastname;    
                    var issuccess = _db.Users.FirstOrDefault(c => c.Email == a && c.Password == b);
                    if (issuccess != null)
                    {
                        //TempData["success"] = "login successful";
                        _contextAccessor.HttpContext.Session.SetString("FirstName", issuccess.FirstName + " " + issuccess.LastName);
                        _contextAccessor.HttpContext.Session.SetString("UserId" , issuccess.UserId.ToString());
                        _contextAccessor.HttpContext.Session.SetString("Email" , issuccess.Email);
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
        public IActionResult registration(RegistrationVm obj)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    PhoneNumber = obj.PhoneNumber,
                    Email = obj.Email,
                    Password = obj.Password,
                    Avatar = "None",
                    //WhyIVolunteer = "None",
                    //EmployeeId = "None",
                    //Department = "None",
                    //ProfileText = "None",
                    //LinkedInUrl = "None",
                    //Title = "None",
                    //UpdatedAt = DateTime.Now,
                    //DeletedAt = DateTime.Now,
                    CityId = 2,
                    CountryId = 3,
                };
                _db.Users.Add(data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                ModelState.AddModelError("Email", "Email Already exists");
            }

            return View();
        }
        //registration

        //forgot password
        [HttpPost]
        
        public IActionResult forgot(User obj)
        {
            var verify = _db.Users.ToList()
                            .Where(a => a.Email == obj.Email).Count();
            if (verify != 0)
            {
                try
                {
                    string token = Guid.NewGuid().ToString();
                    string email = obj.Email;
                    var link = Url.ActionLink("reset", "Home", new { Email =email, Token = token });
                    MailMessage newMail = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com");   
                    newMail.From = new MailAddress("ciplatform333@gmail.com","CI Platform");
                    newMail.To.Add(email);
                    newMail.Subject = "Reset Password Link";
                    newMail.IsBodyHtml = true;
                    newMail.Body = "Please find below the link to reset the password<br><br><br>" + link;
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("ciplatform333@gmail.com", "jbdxshjsnfhyimnp");
                    client.Send(newMail);
                    


                     var  data = new PasswordReset()
                    {
                        Email = email,
                        Token = token
                    };
                    _db.PasswordResets.Add(data);
                    _db.SaveChanges();

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
           
            return View();
        }
        //forgot password

        //reset password
        [HttpGet]
        public IActionResult Reset(string Email, string Token)
        {
            ResetVm vm = new ResetVm()
            { Email = Email, Token = Token };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Reset(ResetVm obj)
        {
            var verify = _db.PasswordResets.Any(a=> a.Email == obj.Email && a.Token == obj.Token);
            if (verify == true)
            {
                var newpass = _db.Users.FirstOrDefault(a => a.Email == obj.Email);
                newpass.Password = obj.Password;
                _db.Users.Update(newpass);
                _db.SaveChanges(true);
                var tokenreset = _db.PasswordResets.Where(a => a.Token == obj.Token).FirstOrDefault();
                _db.PasswordResets.Remove(tokenreset);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            
            return View(obj);
        }
      
        //reset password

       
    }
                //logout

               

                //logout
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
