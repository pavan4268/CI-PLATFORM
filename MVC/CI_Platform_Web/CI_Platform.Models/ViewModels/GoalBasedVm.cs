using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class GoalBasedVm
    {
        public long TimesheetId { get; set; }

        public long? UserId { get; set; }

        public long? MissionId { get; set; }

        public string? MissionName { get; set; }

        public long? Action { get; set; }

        public DateTime DateVolunteered { get; set; }

        public string Date { get; set; }

        public string? Notes { get; set; }

        public string? Status { get; set; }
    }
}
