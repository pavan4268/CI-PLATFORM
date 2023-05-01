using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryDetailsVm
    {
        public string? StoryTitle { get; set; }

        public string? StoryDescription { get; set; }

        public string? Username { get; set; }

        public string? WhyIVolunteer { get; set; }

        public long MissionId { get; set; }

        public List<User>? users { get; set; }

        public long StoryId { get; set; }
    }
}
