using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminTimeSheetRepository
    {
        public List<AdminTimesheetDisplayVm> GetTimeSheets();

        public string ApproveTimeSheet(long timesheetid, long adminid);

        public string DeclineTimeSheet(long timesheetid, long adminid);
    }
}
