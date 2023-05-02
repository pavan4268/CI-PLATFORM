using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class VolunteerMissionVm
    {
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetails { get; set; }

        public int Rating { get; set; } = 0;

        public string? Img { get; set; }

        public string? MissionThemes { get; set; }

        public string? CityName { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public int NumberOfSeats { get; set; } = 0;

        public DateTime? Deadline { get; set; }

        public DateTime? TheEnd { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Seats { get; set; } = 0;

        public int AvailableSeats { get; set; } = 0;

        public string? MissionType { get; set; }

        public long MissionId { get; set; }

        public string? SkillName { get; set; }

        public string? Availability { get; set; }

        public int RatedbyUsers { get; set; } = 0;

        public List<User>? Users { get; set; }

        public List<CommentsVm>? PostedComments { get; set; }

        public string? CommentText { get; set; } 

        public string? UserName { get; set; }

        public List<RecentVolunteersVm>? RecentVolunteers { get; set; }

        public bool IsFavourite { get; set; }

        public List<RelatedMissionsVm>? RelatedMissions { get; set; }

        public int ApplicationStatus { get; set; } = 0;
        
        public int UserRating { get; set; } = 0;

        public string? GoalObjective { get; set; }

        public int GoalValue { get; set; }

        public int GoalAchieved { get; set; } = 0;

        public float GoalPercentage { get; set; }

        public long AlreadyVolunteered { get; set; } = 0;
    }
}
