using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionSkillsDisplayVm
    {
        public long MissionId { get; set; }

        public string? MissionName { get; set; }

        public long SkillId { get; set; }

        public string? SkillName { get; set; }

        public byte Status { get; set; }
    }
}
