using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminCMSRepository
    {
        public List<AdminCMSDisplayVm> GetCms();

        public void AddCMS(AdminCMSCreateVm obj);

        public AdminCMSCreateVm GetCMSDetails(long cmspageid);

        public string SaveEditData(AdminCMSCreateVm obj);

        public string DeleteCMS(long cmspageid);
    }
}
