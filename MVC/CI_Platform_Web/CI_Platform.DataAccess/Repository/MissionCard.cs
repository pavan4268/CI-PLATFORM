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
            var missions = _db.Missions.ToList();
            var cities = _db.Cities.ToList();
            var themes = _db.MissionThemes.ToList();
            var goalMissions = _db.GoalMissions.ToList();
            var missionRatings = _db.MissionRatings.ToList();
            
            foreach (var mission in missions)
            {
                City? city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                MissionTheme missionTheme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                string[] startdatetime = mission.StartDate.ToString().Split(' ');
                string[] enddatetime = mission.EndDate.ToString().Split(' ');
                MissionApplication? applied = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == userid);
                
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
                        Rating = 3,
                        NumberOfSeats = 10,
                        Deadline = startdatetime[0],
                        CreatedAt = mission.CreatedAt,
                        MissionType = mission.MissionType,
                        Seats = (int)mission.TotalSeats,
                        MissionId = mission.MissionId,
                        RegistrationDeadline = mission.StartDate,
                        AvailableSeats = (int)mission.TotalSeats - _db.MissionApplications.Where(x => x.MissionId == mission.MissionId).Count(),

                    };
                    if (applied != null)
                    {
                        cardview.HasApplied = true;
                    }
                    getmissions.Add(cardview);
                }
            }
            return getmissions;
        }
    }
}
