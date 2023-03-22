using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryListingVm
    {
        public string MissionTheme { get; set; }

        public string StoryTitle { get; set; }

        public string StoryDescription { get; set; }

        public string UserName { get; set; }

        public DateTime PublishedAt { get; set; }
    }
}
