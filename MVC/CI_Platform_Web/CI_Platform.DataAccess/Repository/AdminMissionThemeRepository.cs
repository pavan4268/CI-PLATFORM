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
    public class AdminMissionThemeRepository:IAdminMissionThemeRepository
    {
        private readonly CiPlatformDbContext _db;

        public AdminMissionThemeRepository(CiPlatformDbContext db)
        {
            _db = db;
        }

        public List<AdminMissionThemeDisplayVm> GetMissionThemes()
        {
            List<AdminMissionThemeDisplayVm> result = new List<AdminMissionThemeDisplayVm>();
            IEnumerable<MissionTheme> themes = _db.MissionThemes.ToList();
            foreach(var theme in themes)
            {
                AdminMissionThemeDisplayVm vm = new AdminMissionThemeDisplayVm();
                vm.Status = theme.Status;
                vm.Title = theme.Title;
                result.Add(vm);
            }
            return result;
        }
    }
}
