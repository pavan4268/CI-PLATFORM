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

        public List<VolunteerMissionVm> GetMission(long? id, long? userid)
        {
            
            var getmissions = new List<VolunteerMissionVm>();
            var getmission = new VolunteerMissionVm();
            
            var mission = _db.Missions.Where(x=>x.MissionId == id).SingleOrDefault();
            var cities = _db.Cities.ToList();
            var themes = _db.MissionThemes.ToList();
            var goalMissions = _db.GoalMissions.ToList();
            var missionRatings = _db.MissionRatings.ToList();
            var missionapplication = _db.MissionApplications.Where(x=>x.MissionId==mission.MissionId).ToList();
            var missionskill = _db.MissionSkills.Where(x=>x.MissionId==mission.MissionId).ToList();
           
            foreach(var skill in missionskill)
            {
                var user = _db.Skills.Where(x => x.SkillId == skill.SkillId).SingleOrDefault();
            }
            //var skills = _db.Skills.ToList();
            var missionrating = _db.MissionRatings.ToList();

            //for add to favourites
            
            var favourite = _db.FavoriteMissions.Where(x=>x.UserId==userid && x.MissionId==mission.MissionId).FirstOrDefault();
            if (favourite != null)
            {
                getmission.IsFavourite = true;
            }
            else
            {
                getmission.IsFavourite = false;
            }

            //for add to favourites


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
            //var relatedmission = _db.Missions.Include(x => x.City).Include(x => x.Country).Include(x => x.Theme).Where(t => t.MissionId != mission.MissionId && (t.City.Name == city.Name || t.Country.Name == country.Name || t.Theme.Title == missionTheme.Title)).ToList();
            //getmission.RelatedMissionList = relatedmission;
            List<RelatedMissionsVm> relatedmissions = new List<RelatedMissionsVm>();
            var relatedmissionlist = _db.Missions.Where(x=> x.MissionId!=mission.MissionId && (x.CityId==mission.CityId || x.CountryId==mission.CountryId || x.ThemeId==mission.ThemeId)).ToList();
            foreach(var relatedmission in relatedmissionlist)
            {
                RelatedMissionsVm relMissions = new RelatedMissionsVm();
                var cityname = _db.Cities.Where(x => x.CityId == relatedmission.CityId).SingleOrDefault();
                var themename = _db.MissionThemes.Where(x => x.MissionThemeId == relatedmission.ThemeId).SingleOrDefault();
                string[] startdate = relatedmission.StartDate.ToString().Split(' ');
                string[] enddate = relatedmission.EndDate.ToString().Split(' ');
                relMissions.MissionTitle = relatedmission.Title;
                relMissions.MissionDescription = relatedmission.ShortDescription;
                relMissions.CityName = cityname.Name;
                relMissions.MissionTheme = themename.Title;
                relMissions.MissionOrganization = relatedmission.OrganizationName;
                relMissions.SeatsLeft = (int)(relatedmission.TotalSeats - _db.MissionApplications.Where(x => x.MissionId == relatedmission.MissionId).Count());
                relMissions.StartDate = startdate[0];
                relMissions.EndDate = enddate[0];
                relMissions.Deadline = startdate[0];
                relMissions.MissionType = relatedmission.MissionType;
                relMissions.MissionId = relatedmission.MissionId;

                relatedmissions.Add(relMissions);
            }
            getmission.RelatedMissions = relatedmissions;
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
