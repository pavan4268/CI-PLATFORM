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
        private readonly IVolunteerMissionCard _volunteerMissionCard;

       

       

        public MissionController(CiPlatformDbContext db, IMissionCard missionCard, IVolunteerMissionCard volunteerMissionCard)
        {
            _db = db;
            _missionCard = missionCard;
            _volunteerMissionCard = volunteerMissionCard;
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


       
        public IActionResult PlatformLanding(string sortby,string searchquery, int pg=1)
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
                var data1 = cardmodel.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                //sorting logic goes here
                if (sortby != null)
                {
                    List<MissionVm> data = new List<MissionVm>();
                    ViewBag.sortvalue = sortby;
                    if (sortby == "Newest")
                    {
                        data = cardmodel.OrderByDescending(x => x.CreatedAt).ToList();
                    }
                    else if (sortby == "Oldest")
                    {
                        data = cardmodel.OrderBy(x => x.CreatedAt).ToList();
                    }
                    else if (sortby == "Sort By Deadline")
                    {
                        data = cardmodel.OrderBy(x => x.Deadline).ToList();
                    }
                    else if (sortby == "Lowest available seats")
                    {
                        data = cardmodel.OrderBy(x => x.AvailableSeats).ToList();
                    }
                    else if (sortby == "Highest available seats")
                    {
                        data = cardmodel.OrderByDescending(x => x.AvailableSeats).ToList();
                    }
                    //else if (sortby == "Lowest available seats")
                    //{

                    //}
                    //else if (sortby == "Highest available seats")
                    //{

                    //}
                    //else
                    //{

                    //}
                    int recsCount1 = data.Count();
                    var pager1 = new Pager(recsCount1, pg, pagesize);
                    int recSkip1 = (pg - 1) * pagesize;
                    var sortdata = data.Skip(recSkip1).Take(pager1.PageSize).ToList();
                    this.ViewBag.Pager = pager1;
                    return View(sortdata);
                    //sorting logic ends here
                }
                return View(data1);

                
                //return View(cardmodel);


                // sorting logic goes here


                //if (sortby != null)
                //{
                //    List<MissionVm> data = new List<MissionVm>();
                //    ViewBag.sortvalue = sortby;
                //    if (sortby == "Newest")
                //    {
                //        data = cardmodel.OrderByDescending(x => x.CreatedAt).ToList();
                //    }
                //    else if (sortby == "Oldest")
                //    {
                //        data = cardmodel.OrderBy(x => x.CreatedAt).ToList();
                //    }
                //    else if (sortby == "Sort By Deadline")
                //    {
                //        data = cardmodel.OrderBy(x => x.Deadline).ToList();
                //    }
                //    int recsCount1 = data.Count();
                //    var pager1 = new Pager(recsCount1, pg, pagesize);
                //    int recSkip1 = (pg - 1) * pagesize;
                //    var sortdata = data.Skip(recSkip1).Take(pager1.PageSize).ToList();
                //    this.ViewBag.Pager = pager1;
                //    return View(sortdata);
                //}
                //return View(data1);


                //ViewData["Newest"] = String.IsNullOrEmpty(sortby) ? "date_desc" : "";
                //ViewData["Oldest"] = sortby == "Newest" ? "date_desc" : "Date";
                //ViewData["SortByDeadline"] = sortby == "Sort By Deadline" ? "deadline_date" : "Deadline";


                //switch (sortby)
                //{
                //    case "date_desc":
                //        cardmodel = cardmodel.OrderByDescending(x => x.CreatedAt).ToList();
                //        break;

                //    case "date_aecs":
                //        cardmodel = cardmodel.OrderBy(x => x.CreatedAt).ToList();
                //        break;
                //    case "deadline_date":
                //        cardmodel= cardmodel.OrderBy(x => x.Deadline).ToList();
                //        break;
                //}



                // sorting logic ends here


                //search logic goes here

                //if (searchquery != null)
                //{
                //    ViewBag.SearchQuery = searchquery;
                //    var data = cardmodel.Where(x => x.Title.Contains(searchquery)).ToList();
                //    ViewBag.recsCount = data.Count();

                //    if(data.Count == 0)
                //    {
                //        ViewBag.msg = "No mission Found";
                //    }
                //    return View(data);
                //}

                //search logic ends here
                

            }
            return RedirectToAction("Index" , "Home");
        }



        public IActionResult VolunteeringMissionPage(long? id)
        {
            var missionid = _db.Missions.FirstOrDefault(x=>x.MissionId == id);
            var email = HttpContext.Session.GetString("Email");
            ViewBag.missionid = id;
            var mission = _volunteerMissionCard.GetMission(id);
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            ViewBag.UserId = userid;
            return View(mission);  
        }
        [HttpPost]
        public JsonResult AddToFavourite(long? id , long UserId)
        {
            var favouriteMission = _db.FavoriteMissions.ToList();
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            ViewBag.UserId = userid;

            //ViewBag.userid = UserId;
            //var user = _db.Users.FirstOrDefault(x=>x.Email==Email);
            var favMission = favouriteMission.Where(t => t.MissionId == id && t.UserId == UserId).SingleOrDefault();
            
            if (favMission == null)
            {
                FavoriteMission favourite = new FavoriteMission();
                favourite.UserId = userid;
                favourite.MissionId = (long)id;
                _db.FavoriteMissions.Add(favourite);
                _db.SaveChanges();

            }
            else
            {
                _db.FavoriteMissions.Remove(favMission);
                _db.SaveChanges();
            }
            return new JsonResult(favouriteMission);
        }

        [HttpPost]
        public JsonResult DisplayComments(long? id, long UserId, string CommentText)
        {
            var comments = _db.Comments.ToList();
            var commentsDisplay = comments.Where(t => t.MissionId == id).ToList();
            Comment comment = new Comment();
            comment.MissionId = (long)id;
            comment.UserId = UserId;
            comment.CommentText = CommentText;
            _db.Comments.Add(comment);
            _db.SaveChanges();
            
            return new JsonResult(commentsDisplay);
        }



    }
}
