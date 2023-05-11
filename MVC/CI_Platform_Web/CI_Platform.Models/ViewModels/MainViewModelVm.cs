using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MainViewModelVm
    {
        public List<MissionVm>? Missions { get; set; }

        public List<NotificationVm>? Notification { get; set; }
    }
}
