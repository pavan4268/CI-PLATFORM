using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminStoryDisplayVm
    {
        public long StoryId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string? Title { get; set; }

        public string? MissionTitle { get; set; }

        public string? UserName { get; set; }
    }
}
