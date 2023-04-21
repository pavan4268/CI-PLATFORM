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
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var timesheets = _userProfile.GetTimesheets(userid);
            return View(timesheets);
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

            //var abc =(from us in _db.UserSkills.Where(x=> x.UserId == userid)
            //         from s in _db.Skills.Where(x => x.SkillId == us.SkillId)
            //         select new
            //         {
            //             SkillId = s.SkillId,
            //             SkillName = s.SkillName
            //         }).ToList();

                    
                

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
        public string ContactUs(string? Subject, string? Message)
        {
            string reply = "";
            if (Subject == null)
            {
                reply = "Please Enter a Subject";
                return reply;
            }
            if(Message == null)
            {
                reply = "Please Enter a Message";
                return reply;
            }
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
                return reply;
            }
            reply = "User Not Found";
            return reply;
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
                    if (missionname != null && missionname.MissionType == "Time" && missionname.StartDate < DateTime.Today)
                    {
                        StoryMissionListVm obj = new StoryMissionListVm();
                        obj.MissionId = missionname.MissionId;
                        obj.MissionName = missionname.Title;
                        missions.Add(obj);
                    }
                }
                else
                {
                    if (missionname != null && missionname.MissionType == "Goal" && missionname.StartDate < DateTime.Today)
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



        [HttpPost]
        public string AddTimeData(long missionidAdd, string timeDateAdd, int timeHrsAdd, int timeMinsAdd, string timeMsgAdd)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            DateTime entereddate = DateTime.Parse(timeDateAdd);
            var umission = _db.Missions.FirstOrDefault(x => x.MissionId == missionidAdd);
            if (umission != null)
            {
                string error = "";
                if (entereddate > umission.StartDate)
                {
                    error = "Please select a date after the start date of the mission";
                }
                else if (entereddate < umission.EndDate)
                {
                    error = "Please select the date before the end date of the mission";
                }
                else if (entereddate == DateTime.Today)
                {
                    error = "Please select a date before today's date";
                }
                else if (timeHrsAdd < 0 || timeHrsAdd > 24)
                {
                    error = "Please Select Hours Between 0 to 24";
                }
                else if (timeMinsAdd < 0 || timeMinsAdd > 60)
                {
                    error = "Please Select Minutes Between 0 to 60";
                }
                else
                {
                    Timesheet obj = new Timesheet();
                    obj.UserId = userid;
                    obj.MissionId = umission.MissionId;
                    TimeSpan time = TimeSpan.FromHours(timeHrsAdd) + TimeSpan.FromMinutes(timeMinsAdd);
                    obj.Time = time;
                    obj.Notes = timeMsgAdd;
                    obj.DateVolunteered = entereddate;


                    _db.Timesheets.Add(obj);
                    _db.SaveChanges(true);
                }
                return error;
            }
            string missionerror = "Please Select a mission";
            return missionerror;
        }




        [HttpPost]
        public string AddGoalData(long goalMissionIdAdd, string goalDateAdd, int goalActionsAdd, string goalMsgAdd)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            DateTime entereddate = DateTime.Parse(goalDateAdd);
            var umission = _db.Missions.FirstOrDefault(x => x.MissionId == goalMissionIdAdd);
            
            if(umission != null)
            {
                
                 string error = "";
                if (entereddate > umission.StartDate)
                {
                    error = "Please select a date after the start date of the mission";
                }
                else if(entereddate < umission.EndDate)
                {
                    error = "Please select the date before the end date of the mission";
                }
                else if(entereddate == DateTime.Today)
                {
                    error = "Please select a date before today's date";
                }
     
                else
                {
                    Timesheet item = new Timesheet();
                    item.UserId = userid;
                    item.MissionId = umission.MissionId;
                    item.DateVolunteered = entereddate;
                    item.Action = goalActionsAdd;
                    item.Notes = goalMsgAdd;
                    _db.Timesheets.Add(item);
                    _db.SaveChanges();
                    return error;
                }
                
            }
            string missionerror = "Please Select a Mission";
            return missionerror;
        }


        public JsonResult GetTimeEdit(long timesheetid)
        {
            var details = _userProfile.EditTimeSheet(timesheetid);
            return new JsonResult(details);
        }

        public JsonResult GetGoalEdit(long timesheetid)
        {
            var details = _userProfile.EditGoalTimeSheet(timesheetid);
            return new JsonResult(details);
        }




        [HttpPost]
        public string EditTimeData(string timeEditDate, int timeEditHours, int timeEditMins, string timeEditMessage, long timeEditId)
        {
            var editmission = _db.Timesheets.FirstOrDefault(x=> x.TimesheetId == timeEditId);
            DateTime entereddate = DateTime.Parse(timeEditDate);
            var mission = _db.Missions.FirstOrDefault(x=> x.MissionId == editmission.MissionId);
            string error = "";
            if (entereddate > mission.StartDate)
            {
                error = "Please select a date after the start date of the mission";
            }
            else if (entereddate < mission.EndDate)
            {
                error = "Please select the date before the end date of the mission";
            }
            else if (entereddate == DateTime.Today)
            {
                error = "Please select a date before today's date";
            }
            else if (timeEditHours < 0 || timeEditHours > 24)
            {
                error = "Please Select Hours Between 0 to 24";
            }
            else if (timeEditMins < 0 || timeEditMins > 60)
            {
                error = "Please Select Minutes Between 0 to 60";
            }
            else
            {
                TimeSpan time = TimeSpan.FromHours(timeEditHours) + TimeSpan.FromMinutes(timeEditMins);
                editmission.Time = time;
                editmission.Notes = timeEditMessage;
                editmission.DateVolunteered = entereddate;


                _db.Timesheets.Update(editmission);
                _db.SaveChanges(true);
            }
            return error;
        }


        [HttpPost]
        public string EditGoalData(string goalEditDate, int goalEditAction , string goalEditMessage, long goalTimesheetId)
        {
            var editmission = _db.Timesheets.FirstOrDefault(x=> x.TimesheetId == goalTimesheetId);
            DateTime entereddate = DateTime.Parse(goalEditDate);
            var mission = _db.Missions.FirstOrDefault(x=>x.MissionId == editmission.MissionId);
            string error = "";
            if (entereddate > mission.StartDate)
            {
                error = "Please select a date after the start date of the mission";
            }
            else if (entereddate < mission.EndDate)
            {
                error = "Please select the date before the end date of the mission";
            }
            else if (entereddate == DateTime.Today)
            {
                error = "Please select a date before today's date";
            }

            else
            {
                
                editmission.DateVolunteered = entereddate;
                editmission.Action = goalEditAction;
                editmission.Notes = goalEditMessage;
                _db.Timesheets.Update(editmission);
                _db.SaveChanges();
                
            }
            return error;
        }


        [HttpPost]
        public string TimeDelete(long timesheetid)
        {
            var delete = _db.Timesheets.FirstOrDefault(x=>x.TimesheetId==timesheetid);
            string result = "";
            if(delete != null)
            {
                _db.Timesheets.Remove(delete);
                _db.SaveChanges();
                result = "Timesheet Deleted Successfully";
                return result;
            }
            result = "Some Error Occured";
            return result;
        }



        [HttpPost]
        public string GoalDelete(long timesheetid)
        {
            var delete = _db.Timesheets.FirstOrDefault(x => x.TimesheetId == timesheetid);
            string result = "";
            if (delete != null)
            {
                _db.Timesheets.Remove(delete);
                _db.SaveChanges();
                result = "Timesheet Deleted Successfully";
                return result;
            }
            result = "Some Error Occured";
            return result;
        }



    }
}
