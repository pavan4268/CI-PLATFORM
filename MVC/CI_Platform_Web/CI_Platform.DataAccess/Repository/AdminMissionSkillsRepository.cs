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
    public class AdminMissionSkillsRepository: IAdminMissionSkillsRepository
    {
        private readonly CiPlatformDbContext _db;


        public AdminMissionSkillsRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminMissionSkillsDisplayVm> GetMissionSkills()
        {
            List<AdminMissionSkillsDisplayVm> list = new List<AdminMissionSkillsDisplayVm>();
            
            List<MissionSkill> missionskills = _db.MissionSkills.ToList();
            foreach (MissionSkill missionskill in missionskills)
            {
                
                Mission mission = _db.Missions.FirstOrDefault(x=>x.MissionId == missionskill.MissionId);
                Skill skill = _db.Skills.FirstOrDefault(x=>x.SkillId == missionskill.SkillId);
                AdminMissionSkillsDisplayVm vm = new AdminMissionSkillsDisplayVm();
                vm.MissionId = missionskill.MissionId;
                vm.SkillId = missionskill.SkillId;
                vm.Status = skill.Status;
                vm.MissionName = mission.Title;
                vm.SkillName = skill.SkillName;
                list.Add(vm);
                

            }
            return list;
        }

    }
}
