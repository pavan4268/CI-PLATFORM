using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimesheetVm
    {
        public List<TimeBasedVm> TimeBasedSheets { get; set; }

        public List<GoalBasedVm> GoalBasedSheets { get; set; }
    }
}
