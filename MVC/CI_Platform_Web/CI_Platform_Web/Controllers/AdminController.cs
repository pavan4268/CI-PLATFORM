using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform_Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly CiPlatformDbContext _db;
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IAdminCMSRepository _adminCMSRepository;
        private  readonly IAdminMissionRepository _adminMissionRepository;
        private readonly IAdminMissionThemeRepository _adminMissionThemeRepository;
        private readonly IAdminMissionSkillsRepository _adminMissionSkillsRepository;
        private readonly IAdminMissionApplicationRepository _adminMissionApplicationRepository;

        public AdminController(CiPlatformDbContext db, IAdminUserRepository adminUserRepository, IAdminCMSRepository adminCMSRepository, IAdminMissionRepository adminMissionRepository, IAdminMissionThemeRepository adminMissionThemeRepository, IAdminMissionSkillsRepository adminMissionSkillsRepository, IAdminMissionApplicationRepository adminMissionApplicationRepository)
        {
            _db = db;
            _adminUserRepository = adminUserRepository;
            _adminCMSRepository = adminCMSRepository;
            _adminMissionRepository = adminMissionRepository;
            _adminMissionThemeRepository = adminMissionThemeRepository;
            _adminMissionSkillsRepository = adminMissionSkillsRepository;
            _adminMissionApplicationRepository = adminMissionApplicationRepository;
        }






        public IActionResult AdminUserHome()
        {
            List<AdminUserDisplayVm> userdetails = _adminUserRepository.GetUsers();
            return View(userdetails);
        }


        public IActionResult AdminCMSHome()
        {
            List<AdminCMSDisplayVm> cmspages = _adminCMSRepository.GetCms();
            return View(cmspages);
        }


        public IActionResult AdminMissionHome()
        {
            List<AdminMissionDisplayVm> missions = _adminMissionRepository.GetMissions();
            return View(missions);
        }


        public IActionResult AdminMissionThemeHome()
        {
            List<AdminMissionThemeDisplayVm> themes = _adminMissionThemeRepository.GetMissionThemes();
            return View(themes);
        }


        public IActionResult AdminMissionSkillsHome()
        {
            List<AdminMissionSkillsDisplayVm> skills = _adminMissionSkillsRepository.GetMissionSkills();
            return View(skills);
        }


        public IActionResult AdminMissionApplicationHome()
        {
            List<AdminMissionApplicationDisplayVm> applications = _adminMissionApplicationRepository.GetMissionApplications();
            return View(applications);
        }
        //public IActionResult UserHomePage()
        //{
        //    List<AdminUserDisplay> userdetails = _adminUserRepository.GetUsers();
        //    return PartialView("_UserAdminHome", userdetails);
        //}

        
    }
}
