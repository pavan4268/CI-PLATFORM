using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionApplicationDisplayVm
    {
        public long MissionApplicationId { get; set; }

        public string? MissionName { get; set; }

        public string? UserName { get; set; }

        public long MissionId { get; set; }

        public long UserId { get; set; }

        public string? AppliedAt { get; set; }
    }
}
