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
    public class AdminMissionRepository
    {
        private readonly CiPlatformDbContext _db;


        public AdminMissionRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        //public List<AdminMissionDisplayVm> GetMissions()
        //{
        //    List<AdminMissionDisplayVm> missionList = new List<AdminMissionDisplayVm>();
        //    IEnumerable<Mission> missions = _db.Missions.ToList();
        //}
    }
}
