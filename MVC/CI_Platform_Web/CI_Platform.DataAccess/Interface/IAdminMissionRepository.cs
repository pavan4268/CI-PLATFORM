using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionRepository
    {
        public List<AdminMissionDisplayVm> GetMissions();

        public AdminMissionCreateVm FillDropDown();

        public string AddMission(AdminMissionCreateVm obj);

        public AdminMissionCreateVm GetData(long missionid);

        public string EditMission(AdminMissionCreateVm obj);

        public string DeleteMission(long? missionid);
    }
}
