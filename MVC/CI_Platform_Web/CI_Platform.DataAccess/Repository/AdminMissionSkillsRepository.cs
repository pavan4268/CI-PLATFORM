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


        //public List<AdminMissionSkillsDisplayVm> GetMissionSkills()
        //{
        //    List<AdminMissionSkillsDisplayVm> list = new List<AdminMissionSkillsDisplayVm>();
            
        //    List<MissionSkill> missionskills = _db.MissionSkills.ToList();
        //    foreach (MissionSkill missionskill in missionskills)
        //    {
                
        //        Mission mission = _db.Missions.FirstOrDefault(x=>x.MissionId == missionskill.MissionId);
        //        Skill skill = _db.Skills.FirstOrDefault(x=>x.SkillId == missionskill.SkillId);
        //        AdminMissionSkillsDisplayVm vm = new AdminMissionSkillsDisplayVm();
        //        vm.MissionId = missionskill.MissionId;
        //        vm.SkillId = missionskill.SkillId;
        //        vm.Status = skill.Status;
        //        vm.MissionName = mission.Title;
        //        vm.SkillName = skill.SkillName;
        //        list.Add(vm);
                

        //    }
        //    return list;
        //}

        public List<AdminMissionSkillsDisplayVm> GetMissionSkills()
        {
            List<AdminMissionSkillsDisplayVm> result = new List<AdminMissionSkillsDisplayVm>();
            List<Skill> skills = _db.Skills.Where(x=>x.DeletedAt==null).ToList();
            foreach (Skill skill in skills)
            {
                AdminMissionSkillsDisplayVm vm = new AdminMissionSkillsDisplayVm();
                vm.Status = skill.Status;
                vm.SkillName = skill.SkillName;
                vm.SkillId = skill.SkillId;
                result.Add(vm);
            }
            return result;
        }


        public string AddSkill(AdminMissionSkillsCreateVm obj)
        {
            string reply = string.Empty;
            if(obj != null)
            {
                Skill addskill = new Skill();
                addskill.SkillName = obj.SkillName;
                addskill.Status = obj.Status;
                _db.Skills.Add(addskill);
                _db.SaveChanges();
                reply = "Skill Added Successfully";
                return reply;
            }
            return reply;
        }


        public AdminMissionSkillsCreateVm GetSkills(long skillid)
        {
            AdminMissionSkillsCreateVm vm = new AdminMissionSkillsCreateVm();
            Skill? getskill = _db.Skills.FirstOrDefault(x=>x.SkillId == skillid);
            if(getskill != null)
            {
                vm.SkillId = getskill.SkillId;
                vm.SkillName = getskill.SkillName;
                vm.Status=getskill.Status;
                return vm;
            }
            return vm;
        }


        public string EditSkill(AdminMissionSkillsCreateVm obj)
        {
            string reply = string.Empty;
            Skill? editskill = _db.Skills.FirstOrDefault(x=>x.SkillId==obj.SkillId);
            if(editskill != null)
            {
                editskill.Status = obj.Status;
                editskill.SkillName = obj.SkillName;
                editskill.UpdatedAt = DateTime.Now;
                _db.Skills.Update(editskill);
                _db.SaveChanges();
                reply = "Skill Edited Successfully";
                return reply;
            }
            return reply;
        }


        public string DeleteSkill(long skillid)
        {
            string reply = string.Empty;
            Skill? deleteskill = _db.Skills.FirstOrDefault(x=>x.SkillId==skillid);
            if(deleteskill != null)
            {
                MissionSkill? chekcmission = _db.MissionSkills.FirstOrDefault(x=>x.SkillId==deleteskill.SkillId);
                UserSkill? checkuser = _db.UserSkills.FirstOrDefault(x=>x.SkillId==deleteskill.SkillId);
                if(chekcmission != null || checkuser != null)
                {
                    reply = "Cannot Delete Skill, as the skill is associated with the user or a mission";
                    return reply;
                }
                deleteskill.DeletedAt = DateTime.Now;
                _db.Skills.Update(deleteskill);
                _db.SaveChanges(true);
                reply = "Skill Deleted Successfully";
                return reply;
            }
            return reply;
        }
    }
}
