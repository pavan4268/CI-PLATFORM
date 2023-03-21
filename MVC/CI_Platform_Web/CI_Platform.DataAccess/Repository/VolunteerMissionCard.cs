using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
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

        public List<VolunteerMissionVm> GetMission(long? id)
        {
            
            var getmissions = new List<VolunteerMissionVm>();
            var getmission = new VolunteerMissionVm();
            
            var mission = _db.Missions.Where(x=>x.MissionId == id).SingleOrDefault();
            var cities = _db.Cities.ToList();
            var themes = _db.MissionThemes.ToList();
            var goalMissions = _db.GoalMissions.ToList();
            var missionRatings = _db.MissionRatings.ToList();
            var missionapplication = _db.MissionApplications.Where(x=>x.MissionId==mission.MissionId).ToList();
            var missionskill = _db.MissionSkills.ToList();
            //var skills = _db.Skills.ToList();
            var missionrating = _db.MissionRatings.ToList();



            //for displaying comments

            var comments = _db.Comments.ToList();
            var displaycomments = comments.Where(x => x.MissionId == mission.MissionId).ToList();
            
            List<CommentsVm> viewcomments = new List<CommentsVm>();
            foreach (var comment in displaycomments)
            {
                CommentsVm singlecomment = new CommentsVm();
                var user = _db.Users.Where(x=>x.UserId == comment.UserId).FirstOrDefault();
                singlecomment.UserName = user.FirstName+ " " + user.LastName;
                singlecomment.CommentText = comment.CommentText;
                singlecomment.CreatedAt = comment.CreatedAt;
                viewcomments.Add(singlecomment);
            }
            getmission.PostedComments = viewcomments;

            //for displaying comments ends


            //for recent volunteers

            List<RecentVolunteersVm> recentVolunteer = new List<RecentVolunteersVm>();
            foreach (var volunteer in missionapplication)
            {
                RecentVolunteersVm recentParticipant = new RecentVolunteersVm();
                var recentuser = _db.Users.Where(x=>x.UserId==volunteer.UserId).FirstOrDefault();
                recentParticipant.UserName = recentuser.FirstName + " " + recentuser.LastName;
                recentParticipant.AppliedAt = volunteer.CreatedAt;
                recentVolunteer.Add(recentParticipant);
            }
            getmission.RecentVolunteers = recentVolunteer;

            //for recent volunteers ends


            City city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
            MissionTheme missionTheme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
            string[] startdatetime = mission.StartDate.ToString().Split(' ');
            string[] enddatetime = mission.EndDate.ToString().Split(' ');
            var missionskills = missionskill.Where(x => x.MissionId == mission.MissionId).FirstOrDefault();
            //var skill = skills.Where(x => x.SkillId == missionskills.SkillId).FirstOrDefault();
            var skills = _db.Skills.Where(x => x.SkillId == missionskills.SkillId).FirstOrDefault();
            //var ratings = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Count() - _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId())
            Country country = _db.Countries.Where(e=>e.CountryId==mission.CountryId).FirstOrDefault();

            //to display avg rating
            var missionRating = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Average(x => x.Rating);
            var ratingcount = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Count();//this will count total number of users ratings for particular mission

            //for related mission
            //var missionl = _db.Missions.Include(x => x.City).Include(x => x.Country).Include(x => x.Theme).Where(t => t.MissionId == id).SingleOrDefault();
            var relatedmissions = _db.Missions.Include(x => x.City).Include(x => x.Country).Include(x => x.Theme).Where(t => t.MissionId != mission.MissionId && (t.City.Name == city.Name || t.Country.Name == country.Name || t.Theme.Title == missionTheme.Title)).ToList();
            getmission.RelatedMissionList = relatedmissions;
            // for related mission end

            




            

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
                getmission.Rating = (int)missionRating;
                getmission.RatedbyUsers = (int)ratingcount;


           
            
            
            getmissions.Add(getmission);



           




            return getmissions;
        }
    }
}
