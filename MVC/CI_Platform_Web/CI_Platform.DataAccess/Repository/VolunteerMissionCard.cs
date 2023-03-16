using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CI_Platform.Repository.Repository
{
    public class VolunteerMissionCard : IVolunteerMissionCard
    {
        private readonly CiPlatformDbContext _db;

        public VolunteerMissionCard(CiPlatformDbContext db)
        {
            _db = db;
        }

        public VolunteerMissionVm GetMission(long? id)
        {
            
            var getmission = new VolunteerMissionVm();
            var mission = _db.Missions.Where(x=>x.MissionId == id).SingleOrDefault();
            var cities = _db.Cities.ToList();
            var themes = _db.MissionThemes.ToList();
            var goalMissions = _db.GoalMissions.ToList();
            var missionRatings = _db.MissionRatings.ToList();
            var missionapplication = _db.MissionApplications.ToList();
            var missionskill = _db.MissionSkills.ToList();
            //var skills = _db.Skills.ToList();
            var missionrating = _db.MissionRatings.ToList();
            
                City city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                MissionTheme missionTheme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                string[] startdatetime = mission.StartDate.ToString().Split(' ');
                string[] enddatetime = mission.EndDate.ToString().Split(' ');
            var missionskills = missionskill.Where(x => x.MissionId == mission.MissionId).FirstOrDefault();
            //var skill = skills.Where(x => x.SkillId == missionskills.SkillId).FirstOrDefault();
            var skills = _db.Skills.Where(x => x.SkillId == missionskills.SkillId).FirstOrDefault();
            //var ratings = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Count() - _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId())


            getmission.Title = mission.Title;    
            getmission.ShortDescription = mission.ShortDescription;
            getmission.OrganizationName = mission.OrganizationName;
            getmission.MissionThemes = missionTheme.Title;
            getmission.CityName = city.Name;
            getmission.StartDate = "From " + startdatetime[0];
            getmission.EndDate = "Until " + enddatetime[0];
            getmission.Deadline = startdatetime[0];
            getmission.MissionType = mission.MissionType;
            getmission.Seats = (int)mission.TotalSeats;
            getmission.AvailableSeats = (int)mission.TotalSeats - missionapplication.Where(x => x.MissionId == mission.MissionId).Count();
            getmission.SkillName = skills.SkillName;
            
            
            return getmission;
        }
    }
}
