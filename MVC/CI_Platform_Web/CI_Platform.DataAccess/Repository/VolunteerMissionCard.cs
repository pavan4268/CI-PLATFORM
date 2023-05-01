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

            MissionRating? userrating = _db.MissionRatings.FirstOrDefault(rating => rating.MissionId==id && rating.UserId == userid && rating.DeletedAt == null);
            City? city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
            MissionTheme? missionTheme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
            string[] startdatetime = mission.StartDate.ToString().Split(' ');
            string[] enddatetime = mission.EndDate.ToString().Split(' ');
            MissionSkill? missionskills = missionskill.Where(x => x.MissionId == mission.MissionId).FirstOrDefault();
            
            Skill? skills = _db.Skills.Where(x => x.SkillId == missionskills.SkillId).FirstOrDefault();
            //var ratings = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Count() - _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId())
            Country? country = _db.Countries.Where(e=>e.CountryId==mission.CountryId).FirstOrDefault();

            //to display avg rating
            var missionRating = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Average(x => x.Rating);
            var ratingcount = _db.MissionRatings.Where(x=>x.MissionId==mission.MissionId).Count();//this will count total number of users ratings for particular mission

            //for related mission
            
            List<RelatedMissionsVm> relatedmissions = new List<RelatedMissionsVm>();
            var relatedmissionlist = _db.Missions.Where(x=> x.MissionId!=mission.MissionId && (x.CityId==mission.CityId || x.CountryId==mission.CountryId || x.ThemeId==mission.ThemeId)).Take(3).ToList();
            foreach(var relatedmission in relatedmissionlist)
            {
                RelatedMissionsVm? relMissions = new RelatedMissionsVm();
                var cityname = _db.Cities.Where(x => x.CityId == relatedmission.CityId).SingleOrDefault();
                var themename = _db.MissionThemes.Where(x => x.MissionThemeId == relatedmission.ThemeId).SingleOrDefault();
                string[] startdate = relatedmission.StartDate.ToString().Split(' ');
                string[] enddate = relatedmission.EndDate.ToString().Split(' ');
                relMissions.MissionId = relatedmission.MissionId;
                relMissions.MissionTitle = relatedmission.Title;
                relMissions.MissionDescription = relatedmission.ShortDescription;
                relMissions.CityName = cityname.Name;
                relMissions.MissionTheme = themename.Title;
                relMissions.MissionOrganization = relatedmission.OrganizationName;
                relMissions.SeatsLeft = (int)(relatedmission.TotalSeats - _db.MissionApplications.Where(x => x.MissionId == relatedmission.MissionId).Count() == null ? 0: _db.MissionApplications.Where(x => x.MissionId == relatedmission.MissionId).Count());
                relMissions.StartDate = startdate[0];
                relMissions.EndDate = enddate[0];
                relMissions.Deadline = startdate[0];
                relMissions.MissionType = relatedmission.MissionType;
                

                relatedmissions.Add(relMissions);
            }
            getmission.RelatedMissions = relatedmissions;
            // for related mission end







                getmission.MissionId = mission.MissionId;
                getmission.Title = mission.Title;
                getmission.ShortDescription = mission.ShortDescription;
                getmission.OrganizationName = mission.OrganizationName;
                getmission.MissionThemes = missionTheme?.Title;
                getmission.CityName = city.Name;
                getmission.StartDate = "From " + startdatetime[0];
                getmission.EndDate = "Until " + enddatetime[0];
                getmission.MissionType = mission.MissionType;
                if (mission.MissionType == "Time") {    
                        getmission.Deadline = mission.Deadline?.ToString("dd-MM-yyyy");
                        getmission.Seats = (int)mission.TotalSeats;
                        getmission.AvailableSeats = (int)mission.TotalSeats - missionapplication.Where(x => x.MissionId == mission.MissionId).Count();
                }
                if(mission.MissionType == "Goal")
                {
                    GoalMission? getgoaldata = _db.GoalMissions.FirstOrDefault(goal => goal.MissionId == mission.MissionId && goal.DeletedAt==null);
                    if (getgoaldata != null)
                    {
                        getmission.GoalValue = getgoaldata.GoalValue;
                        getmission.GoalObjective = getgoaldata.GoalObjectiveText;
                        List<Timesheet>? getgoalachieved = _db.Timesheets.Where(timesheet=>timesheet.MissionId== mission.MissionId && timesheet.DeletedAt==null).ToList();
                        if (getgoalachieved != null)
                        {
                        getmission.GoalAchieved = (int)getgoalachieved.Sum(goal => goal.Action) == null ? 0 : (int)getgoalachieved.Sum(goal => goal.Action);
                        getmission.GoalPercentage = (float)((float)getmission.GoalAchieved / (float)getmission.GoalValue) * 100;
                        getmission.AlreadyVolunteered = _db.MissionApplications.Count(mission => mission.MissionId == getmission.MissionId && mission.ApprovalStatus == "Approve" && mission.DeletedAt == null); ;
                        }
                    }
                }
                
                
                
                getmission.SkillName = skills.SkillName;
                if(missionRating != null)
                {
                    getmission.Rating = (int)missionRating;
                }
                else
                {
                    getmission.Rating = 0;
                }
                //getmission.Rating = (int)missionRating == null ? 0 : (int)missionRating;
                getmission.RatedbyUsers = (int)ratingcount;
                getmission.ApplicationStatus = GetApplicationStatus(mission.MissionId, userid);
                getmission.Users = _db.Users.Where(user=>user.DeletedAt == null).ToList();
                if(userrating != null)
                {
                    getmission.UserRating = (int)userrating.Rating;
                }
                else
                {
                    getmission.UserRating= 0;
                }
                //getmission.UserRating = (int)userrating.Rating == null ? 0 : (int)userrating.Rating;
            
            
                getmissions.Add(getmission);



           




               return getmissions;
        }
        public int GetApplicationStatus(long? missionid, long? userid)
        {
            MissionApplication? checkapplied = _db.MissionApplications.FirstOrDefault(x => x.MissionId == missionid && x.UserId == userid);
            if(checkapplied != null)
            {
                if(checkapplied.ApprovalStatus == "Pending")
                {
                    return 1;
                }
                if(checkapplied.ApprovalStatus == "Approve")
                {
                    return 2;
                }

            }
            return 0;
        }
    }
}
