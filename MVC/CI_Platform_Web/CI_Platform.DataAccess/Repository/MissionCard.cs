using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class MissionCard : IMissionCard
    {
        private readonly CiPlatformDbContext _db;

        public MissionCard(CiPlatformDbContext db)
        {
            _db = db;
        }

        public List<MissionVm> GetMissions(long? userid)
        {
            var getmissions = new List<MissionVm>();
            var missions = _db.Missions.Where(mission=>mission.DeletedAt==null).ToList();
            var cities = _db.Cities.ToList();
            var themes = _db.MissionThemes.ToList();
            var goalMissions = _db.GoalMissions.ToList();
            var missionRatings = _db.MissionRatings.ToList();
            List<Skill> skills = _db.Skills.ToList();

            foreach (var mission in missions)
            {
                City? city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                MissionTheme missionTheme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                string[] startdatetime = mission.StartDate.ToString().Split(' ');
                string[] enddatetime = mission.EndDate.ToString().Split(' ');
                MissionApplication? applied = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == userid);
                var missionSkill = _db.MissionSkills.Where(e => e.MissionId == mission.MissionId).ToList();
                
                missionSkill = missionSkill.Join(skills, ms => ms.SkillId, s => s.SkillId, (ms, s) => ms).ToList();

                if (city != null)
                {
                    var cardview = new MissionVm
                    {
                        Title = mission.Title,
                        ShortDescription = mission.ShortDescription,
                        OrganizationName = mission.OrganizationName,
                        MissionThemes = missionTheme.Title,
                        CityName = city.Name,
                        StartDate = "From " + startdatetime[0],
                        EndDate = "Until" + enddatetime[0],
                        Img = "~/assets/Grow-Trees-On-the-path-to-environment-sustainability.png",
                        
                        //Deadline = mission.Deadline?.ToString("dd-MM-yyyy"),
                        CreatedAt = mission.CreatedAt,
                        MissionType = mission.MissionType,
                        //Seats = (int)mission.TotalSeats,
                        MissionId = mission.MissionId,
                        //RegistrationDeadline = mission.Deadline,
                        //AvailableSeats = (int)mission.TotalSeats - _db.MissionApplications.Where(x => x.MissionId == mission.MissionId).Count(),
                        CountryId = mission.CountryId,
                        MissionSkill = missionSkill,

                    };
                    if (applied != null)
                    {
                        cardview.HasApplied = true;
                    }
                    if (cardview.MissionType == "Time")
                    {
                        cardview.RegistrationDeadline = mission.Deadline;
                        cardview.Deadline = mission.Deadline?.ToString("dd-MM-yyyy");
                        cardview.Seats = (int)mission.TotalSeats;
                        cardview.AvailableSeats = (int)mission.TotalSeats - _db.MissionApplications.Where(x => x.MissionId == mission.MissionId).Count();
                    }
                    if(cardview.MissionType == "Goal")
                    {
                        GoalMission? getgoaldata = _db.GoalMissions.FirstOrDefault(goal=>goal.MissionId == mission.MissionId && goal.DeletedAt==null);
                        if(getgoaldata != null)
                        {
                            cardview.GoalValue = getgoaldata.GoalValue;
                            cardview.GoalObjective = getgoaldata.GoalObjectiveText;
                            List<Timesheet>? getgoalachieved = _db.Timesheets.Where(timesheet=>timesheet.MissionId == mission.MissionId && timesheet.DeletedAt==null).ToList();
                            if(getgoalachieved != null)
                            {
                                cardview.GoalAchieved = (int)getgoalachieved.Sum(goal => goal.Action) == null ?0 : (int)getgoalachieved.Sum(goal => goal.Action);
                                cardview.GoalPercentage = (float)((float)cardview.GoalAchieved / (float)cardview.GoalValue) * 100;
                                cardview.AlreadyVolunteered = _db.MissionApplications.Count(application => application.MissionId == mission.MissionId && application.ApprovalStatus == "Approve" && application.DeletedAt == null);
                            }
                        }
                        
                        
                    }
                    int? missionrating = (int?)_db.MissionRatings.Where(rating => rating.MissionId == mission.MissionId && rating.DeletedAt == null).Average(rating => rating.Rating);
                    if(missionrating != null)
                    {
                        cardview.Ratings = missionrating;
                    }
                    getmissions.Add(cardview);
                }
            }
            return getmissions;
        }
    }
}
