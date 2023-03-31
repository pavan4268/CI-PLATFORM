using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class VideoListVm
    {
        public string VideoPath { get; set; }

        public long MediaId { get; set; }

        public string Type { get; set; }

        public long StoryId { get; set; }
    }
}
