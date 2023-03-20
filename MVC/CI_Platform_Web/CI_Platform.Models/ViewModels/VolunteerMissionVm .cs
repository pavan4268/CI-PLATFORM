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

        public string ShortDescription { get; set; }

        public string OrganizationName { get; set; }

        public int Rating { get; set; }

        public string Img { get; set; }

        public string MissionThemes { get; set; }

        public string CityName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; } 

        public int NumberOfSeats { get; set; }

        public string Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Seats { get; set; }

        public int AvailableSeats { get; set; }

        public string MissionType { get; set; }

        public long MissionId { get; set; }

        public string SkillName { get; set; }

        public List<Mission> RelatedMissionList { get; set; }

        public int? RatedbyUsers { get; set; } 

        public IEnumerable<User> users { get; set; }

        public List<Comment> PostedComments { get; set; }

        public string CommentText { get; set; } 
    }
}
