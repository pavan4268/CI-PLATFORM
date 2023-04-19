using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionThemeRepository
    {
        public List<AdminMissionThemeDisplayVm> GetMissionThemes();

        public string AddTheme(AdminMissionThemeCreateVm obj);

        public string EditTheme(AdminMissionThemeCreateVm obj);

        public AdminMissionThemeCreateVm GetTheme(long themeid);

        public string DeleteTheme(long themeid);
    }
}
