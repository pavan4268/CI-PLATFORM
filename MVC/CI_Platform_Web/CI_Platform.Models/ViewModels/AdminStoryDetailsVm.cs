using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminStoryDetailsVm
    {
        public long StoryId { get; set; }

        //public long UserId { get; set; }

        //public long MissionId { get; set; }

        public string? Title { get; set; }

        public string? MissionTitle { get; set; }

        public string? UserName { get; set; }

        public List<string>? VideoURLs { get; set; }

        public List<string>? ImagePaths { get; set; }

        public string? StoryDescrription { get; set; }

        public string? UserImage { get; set; }  
    }
}
