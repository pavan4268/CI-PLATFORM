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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IAdminCMSRepository _adminCMSRepository;
        private  readonly IAdminMissionRepository _adminMissionRepository;
        private readonly IAdminMissionThemeRepository _adminMissionThemeRepository;
        private readonly IAdminMissionSkillsRepository _adminMissionSkillsRepository;
        private readonly IAdminMissionApplicationRepository _adminMissionApplicationRepository;

        public AdminController(CiPlatformDbContext db, IAdminUserRepository adminUserRepository, IAdminCMSRepository adminCMSRepository, IAdminMissionRepository adminMissionRepository, IAdminMissionThemeRepository adminMissionThemeRepository, IAdminMissionSkillsRepository adminMissionSkillsRepository, IAdminMissionApplicationRepository adminMissionApplicationRepository, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _hostEnvironment = webHostEnvironment;
            _adminUserRepository = adminUserRepository;
            _adminCMSRepository = adminCMSRepository;
            _adminMissionRepository = adminMissionRepository;
            _adminMissionThemeRepository = adminMissionThemeRepository;
            _adminMissionSkillsRepository = adminMissionSkillsRepository;
            _adminMissionApplicationRepository = adminMissionApplicationRepository;
        }




        //<--------------------------------------------------------------------User----------------------------------------------------------------------------->

        public IActionResult AdminUserHome()
        {
            List<AdminUserDisplayVm> userdetails = _adminUserRepository.GetUsers();
            return View(userdetails);
        }





        public IActionResult AdminUserAdd()
        {
            AdminUserCreateVm vm = _adminUserRepository.Getcountry();
            
            return View(vm);
        }





        [HttpPost]
        public IActionResult AdminUserAdd(AdminUserCreateVm obj, IFormFile? image)
        {
            if(image != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"assets\UserAvatar");
                var extension = Path.GetExtension(image.FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    image.CopyTo(filestream);
                }

                obj.Avatar = fileName + extension;

                //_adminUserRepository.AddUser(obj);
                //return RedirectToAction("AdminUserHome");
            }
            
            

            _adminUserRepository.AddUser(obj);
            //ModelState.AddModelError("EmployeeId", "Enter unique emp id");
            //return View(obj);
            return RedirectToAction("AdminUserHome");

        }




        public JsonResult GetCities(long countryid)
        {
            List<City> cities = _adminUserRepository.GetCities(countryid);
            return new JsonResult(cities);
        }



        public IActionResult AdminUserEdit(long userid)
        {
            AdminUserCreateVm userdetails = _adminUserRepository.GetUser(userid);
            return View(userdetails);
        }

        [HttpPost]
        public IActionResult AdminUserEdit(AdminUserCreateVm obj, long userid, IFormFile image)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"assets\UserAvatar");
            var extension = Path.GetExtension(image.FileName);

            using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                image.CopyTo(filestream);
            }

            //delete existing user avatar if present
            User? edituser = _db.Users.FirstOrDefault(user => user.UserId == userid);
            if(edituser != null && edituser.Avatar!=null)
            {
                string imagepath = edituser.Avatar;
                var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\UserAvatar\" + imagepath));
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            //delete existing user avatar if present
            obj.Avatar = fileName + extension;
            obj.UserId = userid;
            _adminUserRepository.EditUser(obj);
            return RedirectToAction("AdminUserHome");
        }

        
        public string AdminUserDelete(long userid)
        {
            string result = "";

            string? response = _adminUserRepository.DeleteUser(userid);
            if(response != "")
            {
                result = response;
                return result;
            }
            return result;
            
        }
        //<---------------------------------------------------------------------------CMS------------------------------------------------------------------------>


        #region CMS
        public IActionResult AdminCMSHome()
        {
            List<AdminCMSDisplayVm> cmspages = _adminCMSRepository.GetCms();
            return View(cmspages);
        }






        #region CMS ADD
        public IActionResult AdminCMSAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminCMSAdd(AdminCMSCreateVm obj)
        {
            _adminCMSRepository.AddCMS(obj);
            return RedirectToAction("AdminCMSHome");
        }
        #endregion





        #region CMS EDIT
        public IActionResult AdminCMSEdit(long cmspageid)
        {
            AdminCMSCreateVm cms = _adminCMSRepository.GetCMSDetails(cmspageid);
            if (cms != null)
            {
                return View(cms);
            }
            return View();
        }

        
        [HttpPost]
        public IActionResult AdminCMSEdit(AdminCMSCreateVm obj)
        {
            //obj.CmsPageId = cmspageid;
            string? response = _adminCMSRepository.SaveEditData(obj);
            if (response != null)
            {
                return RedirectToAction("AdminCMSHome");
            }
            return View(obj);
        }
        #endregion

        public string AdminCMSDelete(long cmspageid)
        {
            string result = "";
            string? response = _adminCMSRepository.DeleteCMS(cmspageid);
            if(response != null)
            {
                result = response;
                return result;
            }
            return result;
        }
        #endregion





        //<---------------------------------------------------------------------------Mission------------------------------------------------------------------------>
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
