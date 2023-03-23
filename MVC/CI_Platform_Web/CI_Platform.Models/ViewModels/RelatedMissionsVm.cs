using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class RelatedMissionsVm
    {
        public string MissionTitle { get; set; }

        public string MissionDescription { get; set; }

        public string MissionTheme  { get; set; }

        public string MissionOrganization { get; set; }

        public int SeatsLeft { get; set; }

        public string StartDate     { get; set; }

        public string EndDate { get; set; }

        public string Deadline { get; set; }

        public string CityName { get; set; }

        public string MissionType { get; set; }

        public DateTime CreatedAt { get; set; }

        public long MissionId { get; set; }
    }
}
