using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class UserSkillsVm
    {
        public long SkillId { get; set; }

        public string? Skillname { get; set; }

        public long? UserSkillId { get; set; }
    }
}
