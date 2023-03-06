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

        public JsonResult City(int id)
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
            //to get list of the countries

            //var country = _db.Countries.ToList();
            //var country_list = new SelectList(country, "Countryid", "Name");
            //ViewBag.countryl = country_list;
            
            // to get list of the countries

            //to get list of the cities

            var city = _db.Cities.ToList();
            var city_list = new SelectList(city, "Cityid", "Name");
            ViewBag.cityl = city_list;
            
            // to get list of the cities

            // to get the list of mission theme

            List<MissionTheme> missionThemes = new List<MissionTheme>();
            missionThemes = (from a in _db.MissionThemes select a ).ToList();   
            ViewBag.missionThemes = missionThemes;

            //to get list of mission theme

            // to get list of skills 

            List<MissionSkill> missionSkills = new List<MissionSkill>();
            missionSkills = (from a in _db.MissionSkills select a ).ToList();
            ViewBag.missionSkills = missionSkills;

            // to get list of skills

            return View();
        }
   
    }
}
