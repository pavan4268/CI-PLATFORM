using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminBannerRepository
    {
        public List<AdminBannerDisplayVm> GetBanner();

        public string AddBanner(AdminBannerCreateVm obj);

        public AdminBannerCreateVm GetEditData(long bannerid);

        public string SaveEditData(AdminBannerCreateVm obj);

        public string DeleteBanner(long bannerid);
    }
}
