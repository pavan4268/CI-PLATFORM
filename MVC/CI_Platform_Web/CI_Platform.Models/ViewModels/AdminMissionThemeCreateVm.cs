using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionThemeCreateVm
    {
        public long MissionThemeId { get; set; }

        public string? Title { get; set; }

        public byte Status { get; set; }
    }
}
