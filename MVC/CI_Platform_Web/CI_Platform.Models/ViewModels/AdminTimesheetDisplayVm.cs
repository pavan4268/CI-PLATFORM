using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminTimesheetDisplayVm
    {
        public long TimesheetId { get; set; }

        public long? UserId { get; set; }
        
        public string? Username { get; set; }

        public long? MissionId { get; set; }

        public string? MissionTitle { get; set; }

        public TimeSpan? Time { get; set; }

        public long? Action { get; set; }

        public string DateVolunteered { get; set; } = null!;

        public string? Notes { get; set; }

        public string? Status { get; set; }
    }
}
