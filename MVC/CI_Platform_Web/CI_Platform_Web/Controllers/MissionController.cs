using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CI_Platform_Web.Controllers
{
    public class MissionController : Controller
    {
        private readonly CiPlatformDbContext _db;

        public MissionController(CiPlatformDbContext db)
        {
            _db = db;
        }
        public JsonResult Country()
        {
            var cnt = _db.Countries.ToList();
            return new JsonResult(cnt);
        }

        public JsonResult City(long id)
        {
            var cityl = _db.Cities.Where(e => e.CountryId == id).ToList();
            return new JsonResult(cityl);
        }

        public JsonResult Theme()
        {
            var missiontheme = _db.MissionThemes.ToList();
            return new JsonResult(missiontheme);
        }

        public JsonResult Skills()
        {
            var missionskill = _db.Skills.ToList();
            return new JsonResult(missionskill);
        }
        public IActionResult PlatformLanding()
        {
            //if (HttpContext.Session.GetString("FirstName") != null)
            //{
            //    ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            //    return View(ViewBag);


               
            //}
           

            return View();
        }
   
    }
}
