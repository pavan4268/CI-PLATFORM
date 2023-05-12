using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionApplicationRepository
    {
        public List<AdminMissionApplicationDisplayVm> GetMissionApplications();

        public string ApproveApplication(long applicationid, long adminid);

        public string DeclineApplication(long applicationid, long adminid);
    }
}
