using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Repository.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly CiPlatformDbContext _db;
        

        public UserProfileRepository(CiPlatformDbContext db)
        {
            _db = db;
            
        }


        public UserProfileVm GetUserDetails(long userid)
        {
            var currentuser = _db.Users.FirstOrDefault(x => x.UserId == userid);

            if (currentuser != null)
            {
                UserProfileVm userdetails = new UserProfileVm();
                userdetails.Name = currentuser.FirstName;
                userdetails.Surname = currentuser.LastName;
                userdetails.WhyIVolunteer = currentuser.WhyIVolunteer;
                userdetails.Department = currentuser.Department;
                userdetails.EmployeeId = currentuser.EmployeeId;
                userdetails.MyProfile = currentuser.ProfileText;
                userdetails.Title = currentuser.Title;
                userdetails.LinkedIn = currentuser.LinkedInUrl;
                userdetails.countries = _db.Countries.ToList();
                userdetails.CountryId = currentuser.CountryId;
                userdetails.cities = _db.Cities.Where(x => x.CountryId == userdetails.CountryId).ToList();
                userdetails.CityId = currentuser.CityId;
                userdetails.Allskills = _db.Skills.ToList();
                userdetails.Avatar = currentuser.Avatar;
                userdetails.Email = currentuser.Email;
                var userskills = _db.UserSkills.Where(x=>x.UserId==userid).ToList();
                //var example = _db.UserSkills.Include(m=>m.
                List<UserSkillsVm> userskillslist = new List<UserSkillsVm>();
                foreach(var user in userskills)
                {
                    UserSkillsVm skill = new UserSkillsVm();
                    skill.SkillId = user.SkillId;
                    var skillname = _db.Skills.FirstOrDefault(x=>x.SkillId==skill.SkillId);
                    skill.Skillname = skillname.SkillName;
                    skill.UserSkillId = user.UserSkillId;
                    userskillslist.Add(skill);
                }
                userdetails.UserSkills = userskillslist;
                return userdetails;
            }
            return null;
        }


        //public UserProfileVm UpdateUser(UserProfileVm obj, long userid)
        //{
            
        //        var userupdate = _db.Users.FirstOrDefault(x => x.UserId == userid);
        //        userupdate.FirstName = obj.Name;
        //        userupdate.LastName = obj.Surname;
        //        userupdate.WhyIVolunteer = obj.WhyIVolunteer;
        //        userupdate.Department = obj.Department;
        //        userupdate.EmployeeId = obj.EmployeeId;
        //        userupdate.ProfileText = obj.MyProfile;
        //        userupdate.Title = obj.Title;
        //        userupdate.LinkedInUrl = obj.LinkedIn;
        //        userupdate.CountryId = (long)obj.CountryId;
        //        userupdate.CityId = (long)obj.CityId;
        //        _db.Users.Update(userupdate);
        //        _db.SaveChanges();

        //    return 
        //}


        public TimesheetVm GetTimesheets(long userid)
        {
            TimesheetVm timesheetVm = new TimesheetVm();
            var usertimesheets = _db.Timesheets.Where(x=>x.UserId==userid).ToList();
            List<TimeBasedVm> timelist = new List<TimeBasedVm>();
            List<GoalBasedVm> goallist = new List<GoalBasedVm>();
            foreach(var timesheet in usertimesheets)
            {
                var missiondetails = _db.Missions.FirstOrDefault(x=>x.MissionId==timesheet.MissionId);
                if (missiondetails.MissionType == "Time")
                {
                    TimeBasedVm obj = new TimeBasedVm();
                    obj.MissionId = missiondetails.MissionId;
                    obj.MissionName = missiondetails.Title;
                    obj.DateVolunteered = timesheet.DateVolunteered;
                    obj.Date = timesheet.DateVolunteered.ToString("dd-MM-yyyy");
                    obj.TimesheetId = timesheet.TimesheetId;
                    obj.Time = timesheet.Time;
                    timelist.Add(obj);
                }
                else
                {
                    GoalBasedVm obj = new GoalBasedVm();
                    obj.MissionId = missiondetails.MissionId;
                    obj.MissionName = missiondetails.Title;
                    obj.DateVolunteered = timesheet.DateVolunteered;
                    obj.Date = timesheet.DateVolunteered.ToString("dd-MM-yyyy");
                    obj.TimesheetId = timesheet.TimesheetId;
                    obj.Action = timesheet.Action;
                    goallist.Add(obj);
                }
            }
            timesheetVm.TimeBasedSheets = timelist;
            timesheetVm.GoalBasedSheets = goallist;
            return timesheetVm;
        }


        public TimeBasedVm EditTimeSheet(long timesheetid)
        {
            var editdetails = _db.Timesheets.FirstOrDefault(x=> x.TimesheetId == timesheetid);
            TimeBasedVm obj = new TimeBasedVm();
            obj.TimesheetId = editdetails.TimesheetId;
            obj.MissionId= editdetails.MissionId;
            var missionname = _db.Missions.FirstOrDefault(x=> x.MissionId == editdetails.MissionId);
            obj.MissionName = missionname.Title;
            obj.DateVolunteered = editdetails.DateVolunteered;
            obj.Time = editdetails.Time;
            obj.Notes = editdetails.Notes;
            return obj;
        }


        public GoalBasedVm EditGoalTimeSheet(long timesheetid)
        {
            var editdetails = _db.Timesheets.FirstOrDefault(x=> x.TimesheetId == timesheetid);
            GoalBasedVm obj = new GoalBasedVm();
            obj.TimesheetId= editdetails.TimesheetId;
            obj.MissionId = editdetails.MissionId;
            var missionname = _db.Missions.FirstOrDefault(x=> x.MissionId==editdetails.MissionId);
            obj.MissionName = missionname.Title;
            obj.DateVolunteered = editdetails.DateVolunteered;
            obj.Action = editdetails.Action;
            obj.Notes= editdetails.Notes;
            return obj;
        }


        public List<PrivacyPolicyVm> GetCMSData()
        {
            List<PrivacyPolicyVm> obj = new List<PrivacyPolicyVm>();
            List<CmsPage> cms = _db.CmsPages.Where(cms => cms.DeletedAt == null && cms.Status == 1).ToList();
            if(cms.Count > 0)
            {
               
                foreach (var sec in cms)
                {
                    PrivacyPolicyVm section = new PrivacyPolicyVm();
                    section.Title = sec.Title;
                    section.Description = sec.Description;
                    section.CmsPageId = sec.CmsPageId;
                    section.Slug = sec.Slug;
                    obj.Add(section);
                }
            }
            return obj;
            
        }

    }
}
