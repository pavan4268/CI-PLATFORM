using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionSkillsRepository
    {
        public List<AdminMissionSkillsDisplayVm> GetMissionSkills();

        public string AddSkill(AdminMissionSkillsCreateVm obj);

        public AdminMissionSkillsCreateVm GetSkills(long skillid);

        public string EditSkill(AdminMissionSkillsCreateVm obj);

        public string DeleteSkill(long skillid);
    }
}
