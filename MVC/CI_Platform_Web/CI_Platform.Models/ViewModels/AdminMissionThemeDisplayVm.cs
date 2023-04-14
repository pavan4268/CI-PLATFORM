using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionThemeDisplayVm
    {
        

        public long MissionThemeId { get; set; }

        public string? Title { get; set; }

        public byte Status { get; set; }
    }
}
