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
            IEnumerable<MissionTheme> themes = _db.MissionThemes.Where(x=>x.DeletedAt==null).ToList();
            foreach(var theme in themes)
            {
                AdminMissionThemeDisplayVm vm = new AdminMissionThemeDisplayVm();
                vm.Status = theme.Status;
                vm.Title = theme.Title;
                vm.MissionThemeId = theme.MissionThemeId;
                result.Add(vm);
            }
            return result;
        }


        public string AddTheme(AdminMissionThemeCreateVm obj)
        {
            string? reply = "";
            if(obj != null)
            {
                MissionTheme addtheme = new MissionTheme();
                addtheme.Status = obj.Status;
                addtheme.Title = obj.Title;
                _db.MissionThemes.Add(addtheme);
                _db.SaveChanges();
                reply = "Theme Added Successfully";
                return reply;
            }
            return reply;
        }



        public AdminMissionThemeCreateVm GetTheme(long themeid)
        {
            AdminMissionThemeCreateVm vm = new AdminMissionThemeCreateVm();
            MissionTheme? gettheme = _db.MissionThemes.FirstOrDefault(x=>x.MissionThemeId==themeid);
            if(gettheme != null)
            {
                vm.Status = gettheme.Status;
                vm.MissionThemeId=gettheme.MissionThemeId;
                vm.Title = gettheme.Title;
                return vm;
            }
            return vm;
        }



        public string EditTheme(AdminMissionThemeCreateVm obj)
        {
            string reply = string.Empty;
            MissionTheme? edittheme = _db.MissionThemes.Find(obj.MissionThemeId);
            if(edittheme != null)
            {
                edittheme.Status = obj.Status;
                edittheme.Title = obj.Title;
                edittheme.UpdatedAt = DateTime.Now;
                _db.MissionThemes.Update(edittheme);
                _db.SaveChanges(true);
                reply = "Theme Edited Sucessfully";
                return reply;
            }
            return reply;
        }

        public string DeleteTheme(long themeid)
        {
            string reply = string.Empty;
            MissionTheme? deletetheme = _db.MissionThemes.FirstOrDefault(x => x.MissionThemeId == themeid);
            if(deletetheme != null)
            {
                Mission? checkmission = _db.Missions.FirstOrDefault(x=>x.ThemeId==deletetheme.MissionThemeId && x.DeletedAt==null && x.EndDate>DateTime.Today);
                if(checkmission != null)
                {
                    reply = "Can't Delete Theme as it is associated with an ongoing mission";
                    return reply;
                }
                deletetheme.DeletedAt = DateTime.Now;
                _db.MissionThemes.Update(deletetheme);
                _db.SaveChanges();
                reply = "Theme Deleted SucessFully";
                return reply;
            }

            return reply;
        }
    }
}
