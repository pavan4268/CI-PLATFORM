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
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(CiPlatformDbContext db, IUserProfileRepository userProfile, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _userProfile = userProfile;
            _hostEnvironment = hostEnvironment; 
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
        public IActionResult UserProfile(UserProfileVm obj, IFormFile image)
        {
            if(image != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"assets\UserAvatar");
                var extension = Path.GetExtension(image.FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    image.CopyTo(filestream);
                }
                
                obj.Avatar = fileName + extension;

                
                
            }
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            if (obj != null)
            {
                var userupdate = _db.Users.FirstOrDefault(x => x.UserId == userid);
                userupdate.FirstName = obj.Name;
                userupdate.LastName = obj.Surname;
                userupdate.WhyIVolunteer = obj.WhyIVolunteer ?? "None";
                userupdate.Department = obj.Department ?? "None";
                userupdate.EmployeeId = obj.EmployeeId ?? "None";
                userupdate.ProfileText = obj.MyProfile;
                userupdate.Title = obj.Title ?? "None";
                userupdate.LinkedInUrl = obj.LinkedIn ?? "None";
                userupdate.CountryId = (long)obj.CountryId;
                userupdate.CityId = (long)obj.CityId;
                
                if(obj.Avatar != null)
                {
                    userupdate.Avatar = obj.Avatar;
                }
                _db.Users.Update(userupdate);
                _db.SaveChanges();
                
            }
            return RedirectToAction("UserProfile");
        }


        [HttpPost]
        public bool ChangePassword(string cpass, string newpass)
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
                return true;
            }
            //ViewBag.ChangePasswordMessage("Current password is Wrong");
            return false;
            
        }

        [HttpPost]
        public IActionResult SaveUserSkills(long[] skillid)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var remove = _db.UserSkills.Where(x => x.UserId == userid).ToList();
            if (remove.Any())
            {
                _db.UserSkills.RemoveRange(remove);
                _db.SaveChanges();
            }
            foreach (var skill in skillid)
            {
                UserSkill userSkill = new UserSkill();
                userSkill.SkillId = skill;
                userSkill.UserId = userid;
                _db.UserSkills.Add(userSkill);
                _db.SaveChanges();
            }
            var userskills = _userProfile.GetUserDetails(userid);
            return PartialView("_SkillTextArea", userskills);
        }
    }
}
