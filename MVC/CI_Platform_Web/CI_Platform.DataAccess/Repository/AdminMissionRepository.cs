using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Repository.Repository
{
    public class AdminMissionRepository:IAdminMissionRepository
    {
        private readonly CiPlatformDbContext _db;


        public AdminMissionRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminMissionDisplayVm> GetMissions()
        {
            List<AdminMissionDisplayVm> missionList = new List<AdminMissionDisplayVm>();
            List<Mission> missions = _db.Missions.ToList();
            foreach(Mission mission in missions)
            {
                AdminMissionDisplayVm vm = new AdminMissionDisplayVm();
                vm.EndDate = mission.EndDate?.ToString("dd-MM-yyyy");
                vm.MissionId = mission.MissionId;
                vm.StartDate = mission.StartDate?.ToString("dd-MM-yyyy");
                vm.Title = mission.Title;
                vm.MissionType = mission.MissionType;
                missionList.Add(vm);
            }
            return missionList;
        }
    }
}
