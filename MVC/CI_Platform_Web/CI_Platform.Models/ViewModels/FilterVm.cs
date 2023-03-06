using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class FilterVm
    {
        public long SkillId { get; set; }

        public string? SkillName { get; set; }

        public long MissionThemeId { get; set; }

        public string? Title { get; set; }

        public long CityId { get; set; }

        public string Name { get; set; } = null!;

        public long CountryId { get; set; }
    }
}
