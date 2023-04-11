using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult PrivacyPolicy()
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            return View();
        }

        public IActionResult VolunteeringTimesheet()
        {
            return View();
        }

        public JsonResult GetCities(long countryid)
        {
            var cities = _db.Cities.Where(x=> x.CountryId == countryid).ToList();
            return new JsonResult(cities);  
        }


        public IActionResult GetUserSkills()
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);

            var userskills = _db.UserSkills.Where(x => x.UserId == userid).Include(x => x.Skill).ToList();
            var skills = userskills.Select(us => new { SkillId = us.SkillId, SkillName = us.Skill.SkillName });
            return new JsonResult(skills);
        }








        [HttpPost]
        public IActionResult UserProfile(UserProfileVm obj, IFormFile image)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (image != null)
            {
                

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
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction("UserProfile");
            //}
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
                    var imagepath = userupdate.Avatar;
                    var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\UserAvatar\" + imagepath));
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
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



        [HttpPost]
        public bool ContactUs(string? Subject, string? Message)
        {
            string user = HttpContext.Session.GetString("UserId");
            if(user != null)
            {
                long userid = long.Parse(user);
                ContactU item = new ContactU();
                item.UserId = userid;
                item.Subject = Subject;
                item.Message = Message;
                _db.ContactUs.Add(item);
                _db.SaveChanges(true);
                return true;
            }
            return false;
        }

        //<--------------------------------------------------------------volunteer page----------------------------------------------------->


        public JsonResult GetUserTimeMissions(int Type)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var userapplied = _db.MissionApplications.Where(x => x.UserId == userid).ToList();
            List<StoryMissionListVm> missions = new List<StoryMissionListVm>();
            foreach(var item in userapplied)
            {
                var missionname = _db.Missions.Where(x=> x.MissionId==item.MissionId).FirstOrDefault();
                if(Type == 0)
                {
                    if (missionname != null && missionname.MissionType == "Time")
                    {
                        StoryMissionListVm obj = new StoryMissionListVm();
                        obj.MissionId = missionname.MissionId;
                        obj.MissionName = missionname.Title;
                        missions.Add(obj);
                    }
                }
                else
                {
                    if (missionname != null && missionname.MissionType == "Goal")
                    {
                        StoryMissionListVm obj = new StoryMissionListVm();
                        obj.MissionId = missionname.MissionId;
                        obj.MissionName = missionname.Title;
                        missions.Add(obj);
                    }
                }
                
            }
            return new JsonResult(missions);
        }


    }
}
