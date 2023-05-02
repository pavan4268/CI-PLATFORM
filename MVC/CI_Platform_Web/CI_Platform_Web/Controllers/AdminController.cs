using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        private readonly IAdminStoryRepository _adminStoryRepository;
        private readonly IAdminBannerRepository _adminBannerRepository;

        public AdminController(CiPlatformDbContext db, IAdminUserRepository adminUserRepository, IAdminCMSRepository adminCMSRepository, IAdminMissionRepository adminMissionRepository, IAdminMissionThemeRepository adminMissionThemeRepository, IAdminMissionSkillsRepository adminMissionSkillsRepository, IAdminMissionApplicationRepository adminMissionApplicationRepository, IWebHostEnvironment webHostEnvironment, IAdminStoryRepository adminStoryRepository, IAdminBannerRepository adminBannerRepository)
        {
            _db = db;
            _hostEnvironment = webHostEnvironment;
            _adminUserRepository = adminUserRepository;
            _adminCMSRepository = adminCMSRepository;
            _adminMissionRepository = adminMissionRepository;
            _adminMissionThemeRepository = adminMissionThemeRepository;
            _adminMissionSkillsRepository = adminMissionSkillsRepository;
            _adminMissionApplicationRepository = adminMissionApplicationRepository;
            _adminStoryRepository = adminStoryRepository;
            _adminBannerRepository = adminBannerRepository;
        }




        //<--------------------------------------------------------------------User----------------------------------------------------------------------------->
        #region User
        public IActionResult AdminUserHome()
        {
            List<AdminUserDisplayVm> userdetails = _adminUserRepository.GetUsers();
            return View(userdetails);
        }



        #region User Add

        public IActionResult AdminUserAdd()
        {
            AdminUserCreateVm vm = _adminUserRepository.Getcountry();
            
            return View(vm);
        }





        [HttpPost]
        public IActionResult AdminUserAdd(AdminUserCreateVm obj, IFormFile? image)
        {
            AdminUserCreateVm getcountries = _adminUserRepository.Getcountry();



            #region EmployeeId Validation
            if (obj.EmployeeId != null)
            {
                User? checkEmpId = _db.Users.FirstOrDefault(u => u.EmployeeId == obj.EmployeeId);
                if (checkEmpId != null)
                {
                    ModelState.AddModelError("EmployeeId", "Employee Id Already Exists");
                   
                    
                    if(getcountries.Countries.Count > 0)
                    {
                        obj.Countries = getcountries.Countries;
                    }
                    
                    return View(obj);
                }
            }
            #endregion


            #region EmailId Validation

            if(obj.Email != null)
            {
                User? ckeckemail = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
                if (ckeckemail != null)
                {
                    ModelState.AddModelError("Email", "User with the above Email Already Exists.");
                    if(getcountries.Countries.Count > 0)
                    {
                        obj.Countries= getcountries.Countries;  

                    }
                    return View(obj);
                }
            } 

            #endregion

            if (image != null)
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

        #endregion


        public JsonResult GetCities(long countryid)
        {
            List<City> cities = _adminUserRepository.GetCities(countryid);
            return new JsonResult(cities);
        }


        #region User Edit
        public IActionResult AdminUserEdit(long userid)
        {
            AdminUserCreateVm userdetails = _adminUserRepository.GetUser(userid);
            return View(userdetails);
        }

        [HttpPost]
        public IActionResult AdminUserEdit(AdminUserCreateVm obj, long userid, IFormFile? image)
        {
            AdminUserCreateVm getcountries = _adminUserRepository.Getcountry();

            #region EmployeeId Validation
            if (obj.EmployeeId != null)
            {
                User? checkEmpId = _db.Users.FirstOrDefault(u => u.EmployeeId == obj.EmployeeId && u.UserId != userid);
                if (checkEmpId != null)
                {
                    ModelState.AddModelError("EmployeeId", "Employee Id Already Exists");


                    if (getcountries.Countries.Count > 0)
                    {
                        obj.Countries = getcountries.Countries;
                    }

                    return View(obj);
                }
            }
            #endregion


            #region Email Validation
            if (obj.Email != null)
            {
                User? ckeckemail = _db.Users.FirstOrDefault(u => u.Email == obj.Email && u.UserId != userid);
                if (ckeckemail != null)
                {
                    ModelState.AddModelError("Email", "User with the above Email Already Exists.");
                    if (getcountries.Countries.Count > 0)
                    {
                        obj.Countries = getcountries.Countries;

                    }
                    return View(obj);
                }
            }
            #endregion


            #region Saving New Image And Deleting Previous Image
            if (image != null)
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
                if (edituser != null && edituser.Avatar != null)
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
            }
            #endregion


            obj.UserId = userid;
            _adminUserRepository.EditUser(obj);
            return RedirectToAction("AdminUserHome");
        }
        #endregion

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

        #endregion

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

        #region Mission

        public IActionResult AdminMissionHome()
        {
            List<AdminMissionDisplayVm> missions = _adminMissionRepository.GetMissions();
            return View(missions);
        }

        #region Mission Add
        public IActionResult AdminMissionAdd()
        {
            AdminMissionCreateVm vm = _adminMissionRepository.FillDropDown();
            return View(vm);
        }


        [HttpPost]
        public IActionResult AdminMissionAdd(AdminMissionCreateVm obj)
        {
            #region Mission Add Date Validations
            if (obj.StartDate != null)
            {

                List<Country>? Countries = _db.Countries.Where(country => country.DeletedAt == null).ToList();
                List<MissionTheme>? Themes = _db.MissionThemes.Where(theme => theme.Status == 1 && theme.DeletedAt == null).ToList();
                List<Skill>? SkillList = _db.Skills.Where(skill => skill.DeletedAt == null && skill.Status == 1).ToList();
                obj.SkillList = SkillList;
                obj.Countries = Countries;
                obj.MissionThemes = Themes;

                if (obj.StartDate <= DateTime.Today)
                {
                    ModelState.AddModelError("Startdate", "Cannot Insert Today's Date or Date before today's date");
                    return View(obj);
                }
                if(obj.EndDate!= null)
                {
                    if (obj.EndDate <= obj.StartDate)
                    {
                        ModelState.AddModelError("EndDate", "Cannot Insert Date before or equal to StartDate");
                        return View(obj);
                    }
                    if(obj.DeadLine != null)
                    {
                        if(obj.DeadLine >= obj.StartDate)
                        {
                            ModelState.AddModelError("DeadLine", "Cannot Insert Date After or equal to start Date");
                            return View(obj);
                        }
                        if(obj.DeadLine <= DateTime.Today)
                        {
                            ModelState.AddModelError("DeadLine", "Cannot Insert Date before or Equal to today");
                            return View(obj);
                        }
                    }
                }
                //description validation

                if (string.IsNullOrEmpty(obj.Description))
                {
                    ModelState.AddModelError("Description", "Please Enter Description");
                    return View(obj);
                }

                //description validation

            }
            #endregion


            #region Mission Add Images and Docs Add
            if (obj.Images != null)
            {
                List<string> images = new List<string>();
                List<string> docs = new List<string>();
                foreach (var image in obj.Images)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(image.FileName);
                    if(extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        
                        var uploads = Path.Combine(wwwRootPath, @"assets\MissionMedia\Images");
                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            image.CopyTo(filestream);
                        }
                        string img = fileName + extension;
                        images.Add(img);
                        
                    }
                    if (extension == ".pdf" || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".ppt" || extension == ".pptx" || extension == ".xls" || extension == ".xlsx")
                    {
                        
                        var uploads = Path.Combine(wwwRootPath, @"assets\MissionMedia\Documents");
                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            image.CopyTo(filestream);
                        }
                        string doc = fileName + extension;
                        docs.Add(doc);
                        
                    }
                    

                   
                }
                if(images.Count > 0)
                {
                    obj.Imagepaths = images;
                }
                if(docs.Count > 0)
                {
                    obj.Documentpaths = docs;
                }
                
            }
            #endregion

            string? response = _adminMissionRepository.AddMission(obj);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminMissionHome");

            }
            return View(obj);
        }
        #endregion

        #region Mission Edit
        public IActionResult AdminMissionEdit(long missionid)
        {
            AdminMissionCreateVm obj = _adminMissionRepository.GetData(missionid);
            return View(obj);
        }


        
        public bool ImageDelete(long id, string source, string extension)
        {
            MissionMedium? image = _db.MissionMedia.FirstOrDefault(x=>x.MissionId==id && x.MediaPath == source && x.MediaType==extension);
            if (image != null)
            {
                image.DeletedAt = DateTime.Now;
                _db.MissionMedia.Update(image);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        
        public bool DocumentDelete(long id, string source, string extension)
        {
            MissionDocument? document = _db.MissionDocuments.FirstOrDefault(x => x.MissionId == id && x.DocumentPath == source && x.DocumentType == extension);
            if(document != null)
            {
                document.DeletedAt = DateTime.Now;
                _db.MissionDocuments.Update(document);
                _db.SaveChanges();
                return true;
            }
            return false;
        }


        #region Mission Edit Post
        [HttpPost]
        public IActionResult AdminMissionEdit(AdminMissionCreateVm obj)
        {

            #region Mission Edit Date Validations
            if (obj.StartDate != null)
            {


                #region Refill DropDown
                List<Country>? Countries = _db.Countries.Where(country => country.DeletedAt == null).ToList();
                List<MissionTheme>? Themes = _db.MissionThemes.Where(theme => theme.Status == 1 && theme.DeletedAt == null).ToList();
                List<Skill>? SkillList = _db.Skills.Where(skill => skill.DeletedAt == null && skill.Status == 1).ToList();
                obj.SkillList = SkillList;
                obj.Countries = Countries;
                obj.MissionThemes = Themes;
                #endregion

                #region Refill Image and Docs
                List<MissionMedium>? medias = _db.MissionMedia.Where(x => x.MissionId == obj.MissionId && x.DeletedAt == null).ToList();
                if (medias != null)
                {
                    List<string> images = new List<string>();
                    foreach (var media in medias.Where(x => x.MediaType == "img"))
                    {
                        images.Add(media.MediaPath);
                    }
                    obj.Imagepaths = images;
                    MissionMedium? videopath = _db.MissionMedia.FirstOrDefault(x => x.MissionId == obj.MissionId && x.MediaType == "mp4");
                    if (videopath != null)
                    {
                        obj.VideoURL = videopath.MediaPath;
                    }

                }
                List<MissionDocument>? missiondocs = _db.MissionDocuments.Where(x => x.MissionId == obj.MissionId && x.DeletedAt == null).ToList();
                if (missiondocs != null)
                {
                    List<string>? docs = new List<string>();
                    foreach (MissionDocument document in missiondocs)
                    {
                        docs.Add(document.DocumentPath);
                    }
                    obj.Documentpaths = docs;
                }
                #endregion

                Mission? getmission = _db.Missions.FirstOrDefault(mission=>mission.MissionId == obj.MissionId && mission.DeletedAt == null);
                //if(obj.StartDate > DateTime.Today)
                //{
                //    if (obj.StartDate <= DateTime.Today)
                //    {
                //        ModelState.AddModelError("Startdate", "Cannot Insert Today's Date or Date before today's date");
                //        return View(obj);
                //    }
                //}
                if(obj.StartDate != getmission.StartDate)
                {
                    if(getmission.StartDate <= DateTime.Today)
                    {
                        ModelState.AddModelError("StartDate", "Cannot Edit StartDate of an ongoing mission.");
                        return View(obj);
                    }
                    if (obj.StartDate <= DateTime.Today)
                    {
                        ModelState.AddModelError("Startdate", "Cannot Insert Today's Date or Date before today's date");
                        return View(obj);
                    }
                }

                if (obj.EndDate != null)
                {

                    if(obj.EndDate != getmission.EndDate)
                    {
                        if(getmission.EndDate <= DateTime.Today)
                        {
                            ModelState.AddModelError("EndDate", "Cannot Edit Date of Closed Mission");
                            return View(obj);
                        }
                        if (obj.EndDate <= obj.StartDate)
                        {
                            ModelState.AddModelError("EndDate", "Cannot Insert Date before or equal to StartDate");
                            return View(obj);
                        }
                        //if()
                    }
                    
                    
                }
                if (obj.DeadLine != null)
                {
                    if (obj.DeadLine != getmission.Deadline)
                    {
                        if (getmission.StartDate <= DateTime.Today)
                        {
                            ModelState.AddModelError("DeadLine", "Cannot Edit Deadline of an Ongoing Mission");
                            return View(obj);
                        }
                        if (obj.DeadLine >= obj.StartDate)
                        {
                            ModelState.AddModelError("DeadLine", "Cannot Insert Date After or equal to start Date");
                            return View(obj);
                        }
                        if (obj.DeadLine <= DateTime.Today)
                        {
                            ModelState.AddModelError("DeadLine", "Cannot Insert Date before or Equal to today");
                            return View(obj);
                        }
                    }

                }

            }
            #endregion



            if (obj.Images != null)
            {
                List<string> images = new List<string>();
                List<string> docs = new List<string>();
                foreach (var image in obj.Images)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(image.FileName);
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        
                        var uploads = Path.Combine(wwwRootPath, @"assets\MissionMedia\Images");
                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            image.CopyTo(filestream);
                        }
                        string img = fileName + extension;
                        images.Add(img);
                        
                    }
                    if(extension == ".pdf" || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension ==".ppt" || extension == ".pptx" || extension == ".xls" || extension == ".xlsx")
                    {
                        
                        var uploads = Path.Combine(wwwRootPath, @"assets\MissionMedia\Documents");
                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            image.CopyTo(filestream);
                        }
                        string doc = fileName + extension;
                        docs.Add(doc);
                        
                    }



                }
                if(images.Count > 0)
                {
                    obj.Imagepaths = null;
                    obj.Imagepaths = images;
                }
                if(docs.Count > 0)
                {
                    obj.Documentpaths = null;
                    obj.Documentpaths = docs;
                }
                
            }
            string? response = _adminMissionRepository.EditMission(obj);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminMissionHome");
            }
            return View(obj);
        }
        #endregion

        #endregion


        public string DeleteMission(long? missionid)
        {
            string? reply = string.Empty;
            string wwwRootPath = _hostEnvironment.WebRootPath;
            
            if (missionid != null)
            {
                List<string>? images = (from mm in _db.MissionMedia.Where(media => media.MissionId == missionid && media.DeletedAt == null)
                                        select mm.MediaPath).ToList();
                if (images.Count > 0)
                {

                    foreach (string img in images)
                    {
                        //string imagepath = edituser.Avatar;
                        var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\MissionMedia\Images\" + img));
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
                List<string>? documents = (from md in _db.MissionDocuments.Where(document => document.MissionId == missionid && document.DeletedAt == null)
                                           select md.DocumentPath).ToList();
                if (documents.Count > 0)
                {
                    foreach (string document in documents)
                    {
                        var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\MissionMedia\Documents\" + document));
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
                string? response = _adminMissionRepository.DeleteMission(missionid);
                if(response == null)
                {
                    return reply;
                }

                return response;
            }
            reply = "Could not Delete Mission";
            return reply;
        }

        #endregion

        //<------------------------------------------------------------------------Mission Theme---------------------------------------------------------------------->

        #region Mission Theme
        public IActionResult AdminMissionThemeHome()
        {
            List<AdminMissionThemeDisplayVm> themes = _adminMissionThemeRepository.GetMissionThemes();
            return View(themes);
        }

        #region Theme Add
        public IActionResult AdminMissionThemeAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminMissionThemeAdd(AdminMissionThemeCreateVm obj)
        {
            if(obj.Title != null)
            {
                List<MissionTheme>? checktheme = _db.MissionThemes.Where(theme=>theme.DeletedAt==null).ToList();
                if(checktheme.Count > 0)
                {
                    if(checktheme.Any(theme=>theme.Title.ToLower() == obj.Title.ToLower()))
                    {
                        ModelState.AddModelError("Title", "Theme Already Exists");
                    }
                }
            }
           string response =  _adminMissionThemeRepository.AddTheme(obj);
            if (string.IsNullOrEmpty(response))
            {
                return View(obj);
            }
            return RedirectToAction("AdminMissionThemeHome");
        }
        #endregion

        #region Theme Edit
        public IActionResult AdminMissionThemeEdit(long themeid)
        {
            AdminMissionThemeCreateVm vm =_adminMissionThemeRepository.GetTheme(themeid);
            if (vm != null)
            {
                return View(vm);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminMissionThemeEdit(AdminMissionThemeCreateVm obj)
        {
            if (obj.Title != null)
            {
                List<MissionTheme>? checktheme = _db.MissionThemes.Where(theme=>theme.MissionThemeId != obj.MissionThemeId && theme.DeletedAt==null).ToList();
                if(checktheme.Count > 0)
                {
                    if(checktheme.Any(theme=> theme.Title.ToLower() == obj.Title.ToLower()))
                    {
                        ModelState.AddModelError("Title", "Theme Already Exists");
                    }
                }
            }
            string? response = _adminMissionThemeRepository.EditTheme(obj);
            if (string.IsNullOrEmpty(response))
            {
                return View(obj);
            }
            return RedirectToAction("AdminMissionThemeHome");
        }
        #endregion

        public string AdminMissionThemeDelete(long themeid)
        {
            string? reply = string.Empty;
            string? response = _adminMissionThemeRepository.DeleteTheme(themeid);
            if (string.IsNullOrEmpty(response))
            {
                //reply = string.Empty;
                return reply;
            }
            reply = response;
            return reply;
        }

        #endregion

        //<------------------------------------------------------------------------Mission Skills---------------------------------------------------------------------->

        #region Mission Skill

        public IActionResult AdminMissionSkillsHome()
        {
            List<AdminMissionSkillsDisplayVm> skills = _adminMissionSkillsRepository.GetMissionSkills();
            return View(skills);
        }

        #region Mission Skills Add
        public IActionResult AdminMissionSkillsAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminMissionSkillsAdd(AdminMissionSkillsCreateVm obj)
        {
            //Skill? checkskill = _db.Skills.FirstOrDefault(skill=>skill.SkillId == obj.SkillId && skill.DeletedAt == null);
            List<Skill>? checkskill = _db.Skills.Where(skill => skill.DeletedAt == null).ToList(); 
            if(checkskill != null)
            {
                if(checkskill.Any(x=>x.SkillName.ToLower() == obj.SkillName.ToLower()))
                {
                    ModelState.AddModelError("SkillName", "Skill Already Exists");
                    return View(obj);
                }
            }
            string? response = _adminMissionSkillsRepository.AddSkill(obj);
            if (string.IsNullOrEmpty(response))
            {
                return View(obj);
            }
            return RedirectToAction("AdminMissionSkillsHome");
        }
        #endregion


        #region Mission Skills Edit
        public IActionResult AdminMissionSkillsEdit(long skillid)
        {
            

            AdminMissionSkillsCreateVm getskill = _adminMissionSkillsRepository.GetSkills(skillid);
            if (getskill != null)
            {
                return View(getskill);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminMissionSkillsEdit(AdminMissionSkillsCreateVm obj)
        {
            List<Skill>? checkskill = _db.Skills.Where(skill => skill.DeletedAt == null).ToList();
            if(checkskill != null)
            {
                if (checkskill.Any(skill => skill.SkillName.ToLower() == obj.SkillName.ToLower() && skill.SkillId != obj.SkillId))
                {
                    ModelState.AddModelError("SkillName", "Skill Already Exists");
                    return View(obj);
                }
            }
            string? response = _adminMissionSkillsRepository.EditSkill(obj);
            if (string.IsNullOrEmpty(response))
            {
                return View(obj);
            }
            return RedirectToAction("AdminMissionSkillsHome");
        }

        #endregion

        public string AdminMissionSkillsDelete(long skillid)
        {
            string? reply = string.Empty;
            string response = _adminMissionSkillsRepository.DeleteSkill(skillid);
            if (string.IsNullOrEmpty(response))
            {
                return reply;
            }
            reply = response;
            return reply;
        }


        #endregion

        //<------------------------------------------------------------------------Mission Application------------------------------------------------------------------>

        #region Mission Application

        public IActionResult AdminMissionApplicationHome()
        {
            List<AdminMissionApplicationDisplayVm> applications = _adminMissionApplicationRepository.GetMissionApplications();
            return View(applications);
        }
        
        public IActionResult AdminApplicationApprove(long applicationid)
        {
            string response = _adminMissionApplicationRepository.ApproveApplication(applicationid);
            if (string.IsNullOrEmpty(response))
            {
                ViewBag.Message = "An Error occured";
                return RedirectToAction("AdminMissionApplicationHome");

            }
            ViewBag.Message = response;
            return RedirectToAction("AdminMissionApplicationHome");
        }

        public IActionResult AdminApplicationDecline(long applicationid)
        {
            string response = _adminMissionApplicationRepository.DeclineApplication(applicationid);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminMissionApplicationHome");
            }
            return RedirectToAction("AdminMissionApplicationHome");
        }


        #endregion


        //<--------------------------------------------------------------------------------Story------------------------------------------------------------------------>


        #region Story
        public IActionResult AdminStoryHome()
        {
            List<AdminStoryDisplayVm>? stories = _adminStoryRepository.GetStories();
            return View(stories);
        }

        public IActionResult AdminStoryDetails(long storyid)
        {
            AdminStoryDetailsVm details = _adminStoryRepository.GetDetails(storyid);
            return View(details);
        }

        public IActionResult ApproveStory(long storyid)
        {
            
            string? response = _adminStoryRepository.StoryApprove(storyid);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminStoryHome");
            }
            
            return RedirectToAction("AdminStoryHome");
        }

        public IActionResult DeclineStory(long storyid)
        {
            string? response = _adminStoryRepository.StoryDecline(storyid);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminStoryHome");
            }
            return RedirectToAction("AdminStoryHome");
        }


        public string DeleteStory(long storyid)
        {
            string? reply = "";
            string? response = _adminStoryRepository.StoryDelete(storyid);
            if (string.IsNullOrEmpty(response))
            {
                return reply;
            }
            reply = response;
            return reply;
        }

        public IActionResult ViewDelete(long storyid)
        {
            string? response = _adminStoryRepository.StoryDelete(storyid);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminStoryHome");
            }
            return RedirectToAction("AdminStoryHome");
        }


        #endregion


        //<--------------------------------------------------------------------------------Banner---------------------------------------------------------------------->


        #region Banner
        public IActionResult AdminBannerHome()
        {
            List<AdminBannerDisplayVm>? banners = _adminBannerRepository.GetBanner();
            return View(banners);
        }


        #region Banner Add
        public IActionResult AdminBannerAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminBannerAdd(AdminBannerCreateVm obj)
        {
            if(obj.BannerImage == null)
            {
                ModelState.AddModelError("BannerImage", "Please Select an Image");
                return View(obj);
            }
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"assets\Banner");
            var extension = Path.GetExtension(obj.BannerImage.FileName);

            using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                obj.BannerImage.CopyTo(filestream);
            }

            obj.Image = fileName + extension;

            string? response = _adminBannerRepository.AddBanner(obj);
            if (string.IsNullOrEmpty(response))
            {
                return RedirectToAction("AdminBannerHome");
            }
            ViewBag.ErrorMessage = "Some Error occured";
            return View();
        }

        #endregion


        #region Banner Edit
        public IActionResult AdminBannerEdit(long bannerid)
        {
            AdminBannerCreateVm? banner = _adminBannerRepository.GetEditData(bannerid);
            if (banner != null)
            {
                return View(banner);
            }
            TempData["ErrorMessage"] = "Cannot Find Banner Data";
            return RedirectToAction("AdminBannerHome");
        }

        [HttpPost]
        public IActionResult AdminBannerEdit(AdminBannerCreateVm obj)
        {
            if(obj.BannerImage != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"assets\Banner");
                var extension = Path.GetExtension(obj.BannerImage.FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    obj.BannerImage.CopyTo(filestream);
                }

                obj.Image = fileName + extension;
            }
            string? response = _adminBannerRepository.SaveEditData(obj);
            if (string.IsNullOrEmpty(response))
            {
                TempData["SuccessMessage"] = "Banner Edited SuccessFully";
                return RedirectToAction("AdminBannerHome");
            }
            ViewBag.ErrorMessage = response;
            return View();
        }
        #endregion

        public string DeleteBanner(long bannerid)
        {
            string? reply = "";
            Banner? deleteimg = _db.Banners.FirstOrDefault(banner=>banner.BannerId == bannerid && banner.DeletedAt == null);
            if(deleteimg != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagepath = deleteimg.Image;
                var filepath = Path.GetFullPath(Path.Combine(wwwRootPath, @"assets\Banner\" + imagepath));
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            string? response = _adminBannerRepository.DeleteBanner(bannerid);
            if (string.IsNullOrEmpty(response))
            {
                return reply;
            }
            return response;
        }
        #endregion

    }
}
