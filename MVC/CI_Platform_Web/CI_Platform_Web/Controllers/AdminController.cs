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

        public AdminController(CiPlatformDbContext db, IAdminUserRepository adminUserRepository, IAdminCMSRepository adminCMSRepository)
        {
            _db = db;
            _adminUserRepository = adminUserRepository;
            _adminCMSRepository = adminCMSRepository;
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
            return View();
        }


        //public IActionResult UserHomePage()
        //{
        //    List<AdminUserDisplay> userdetails = _adminUserRepository.GetUsers();
        //    return PartialView("_UserAdminHome", userdetails);
        //}

        
    }
}
