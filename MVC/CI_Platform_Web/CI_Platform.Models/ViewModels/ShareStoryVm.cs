using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class ShareStoryVm
    {
        public int MissionId { get; set; }

        public string StoryTitle { get; set; }

        public DateTime Date { get; set; }

        public string StoryDesctiption { get; set; }

        public string VideoUrl { get; set; }

        public string ImagePath { get; set; }

        public string StoryStatus { get; set; }

        public List<StoryMissionListVm> UserAppliedMissions { get; set; }

        public List<IFormFile>? Storyimg { get; set; }

        

    }
}
