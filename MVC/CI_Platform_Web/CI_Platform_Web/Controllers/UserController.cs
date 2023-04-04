using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System.Net.Mail;

namespace CI_Platform_Web.Controllers
{
    public class UserController : Controller
    {

        private readonly CiPlatformDbContext _db;
        private readonly IUserProfileRepository _userProfile;

        public UserController(CiPlatformDbContext db, IUserProfileRepository userProfile)
        {
            _db = db;
            _userProfile = userProfile;
        }


        public IActionResult UserProfile()
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var userdetails = _userProfile.GetUserDetails(userid);
            return View(userdetails);
        }


        public JsonResult GetCities(long countryid)
        {
            var cities = _db.Cities.Where(x=> x.CountryId == countryid).ToList();
            return new JsonResult(cities);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfileVm obj)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            if (obj != null)
            {
                var userupdate = _db.Users.FirstOrDefault(x => x.UserId == userid);
                userupdate.FirstName = obj.Name;
                userupdate.LastName = obj.Surname;
                userupdate.WhyIVolunteer = obj.WhyIVolunteer;
                userupdate.Department = obj.Department;
                userupdate.EmployeeId = obj.EmployeeId;
                userupdate.ProfileText = obj.MyProfile;
                userupdate.Title = obj.Title;
                userupdate.LinkedInUrl = obj.LinkedIn;
                userupdate.CountryId = (long)obj.CountryId;
                userupdate.CityId = (long)obj.CityId;
                _db.Users.Update(userupdate);
                _db.SaveChanges();
                
            }
            return RedirectToAction("UserProfile");
        }


        [HttpPost]
        public IActionResult ChangePassword(string cpass, string newpass)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var userpass = _db.Users.FirstOrDefault(x => x.UserId == userid);
            if (userpass.Password == cpass)
            {
                userpass.Password = newpass;
                _db.Update(userpass);
                _db.SaveChanges(true);
                //ViewBag.ChangePasswordSuccess("Password Changed Sucessfully");
                return RedirectToAction("UserProfile");
            }
            ViewBag.ChangePasswordMessage("Current password is Wrong");
            return null;
            
        }
    }
}
