using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace CI_Platform_Web.Controllers
{
    public class MissionController : Controller
    {
        private readonly CiPlatformDbContext _db;
        private readonly IMissionCard _missionCard;

       

       

        public MissionController(CiPlatformDbContext db, IMissionCard missionCard)
        {
            _db = db;
            _missionCard = missionCard;
        }

        //filter dropdowns


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

        // filter dropdowns


       
        public IActionResult PlatformLanding(int pg=1)
        {

            if (HttpContext.Session.GetString("FirstName") != null)
            {
                var cardmodel = _missionCard.GetMissions();

                const int pagesize = 3;
                if (pg < 1)
                {
                    pg = 1;
                }

                int recsCount = cardmodel.Count();
                 var pager = new Pager(recsCount,pg,pagesize);
                int recSkip = (pg - 1) * pagesize;
                var data = cardmodel.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(data);


                //return View(cardmodel);
            }
            return RedirectToAction("Index" , "Home");
        }
       
   
    }
}
