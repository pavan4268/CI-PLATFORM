﻿using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Net.Mail;

namespace CI_Platform_Web.Controllers
{
    public class MissionController : Controller
    {
        private readonly CiPlatformDbContext _db;
        private readonly IMissionCard _missionCard;
        private readonly IVolunteerMissionCard _volunteerMissionCard;
        private readonly INotificationRepository _notification;

       

       

        public MissionController(CiPlatformDbContext db, IMissionCard missionCard, IVolunteerMissionCard volunteerMissionCard, INotificationRepository notification)
        {
            _db = db;
            _missionCard = missionCard;
            _volunteerMissionCard = volunteerMissionCard;
            _notification = notification;
        }

        //filter dropdowns


        public JsonResult Country()
        {
            var cnt = _db.Countries.Where(country => country.DeletedAt == null).ToList();
            return new JsonResult(cnt);
        }

        public JsonResult City(long id)
        {
            var cityl = _db.Cities.Where(e => e.CountryId == id && e.DeletedAt==null).ToList();
            return new JsonResult(cityl);
        }

        public JsonResult Theme()
        {
            var missiontheme = _db.MissionThemes.Where(theme => theme.DeletedAt == null && theme.Status == 1).ToList();
            return new JsonResult(missiontheme);
        }

        public JsonResult Skills()
        {
            var missionskill = _db.Skills.Where(skill => skill.DeletedAt == null && skill.Status == 1).ToList();
            return new JsonResult(missionskill);
        }

        // filter dropdowns


       
        public IActionResult PlatformLanding(string sortby, int pg=1)
        {

            if (HttpContext.Session.GetString("FirstName") != null)
            {
                string user = HttpContext.Session.GetString("UserId");
                long userid = long.Parse(user);
                var cardmodel = _missionCard.GetMissions(userid);
                MainViewModelVm vm = new MainViewModelVm();
                vm.Notification = _notification.GetNotifications(userid);
                
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
                    else if(sortby == "My favourites")
                    {
                        data = cardmodel.OrderBy(card=>card.IsFavourite).ToList();
                    }
                    
                    int recsCount1 = data.Count();
                    
                    var pager1 = new Pager(recsCount1, pg, pagesize);
                    int recSkip1 = (pg - 1) * pagesize;
                    var sortdata = data.Skip(recSkip1).Take(pager1.PageSize).ToList();
                    this.ViewBag.Pager = pager1;
                    vm.Missions = sortdata;
                    return View(vm);
                    //sorting logic ends here
                }
                ViewBag.NumberofMissions = recsCount;
                vm.Missions = data1;
                return View(vm);

                
                //return View(cardmodel);


               


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

        #region Mission Filter
        public IActionResult FilterMissions(int countryId, string sort, string SearchText, string[] cities, string[] themes, string[] skills)
        {



            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var data = _missionCard.GetMissions(userid);
            //var data = _userRepositery.GetMissions(HttpContext.Session.GetString("userId"));

            if (countryId > 0)
            {
                data = data.Where(x => x.CountryId == countryId).ToList();

                if (data.Count == 0)
                {
                    return PartialView("_NoMissionPartial");
                }





            }
            if (cities.Length > 0)
            {

                data = data.Where(c => cities.Contains(c.CityName)).ToList();

                if (data.Count == 0)
                {
                    return PartialView("_NoMissionPartial");
                }
            }

            if (themes.Length > 0)
            {


                data = data.Where(c => themes.Contains(c.MissionThemes)).ToList();

                if (data.Count == 0)
                {
                    return PartialView("_NoMissionPartial");
                }
            }



            if (skills.Length > 0)
            {


                data = data.Where(c => skills.Contains(c.MissionSkill.ToString())).ToList();


                if (data.Count == 0)
                {
                    return PartialView("_NoMissionPartial");
                }
            }


            if (sort != null)
            {



                if (sort == "Newest")
                {
                    data = data.OrderByDescending(x => x.CreatedAt).ToList();

                }
                else if (sort == "Oldest")
                {
                    data = data.OrderBy(x => x.CreatedAt).ToList();

                }

                else if (sort == "Lowest available seats")
                {
                    data = data.OrderBy(x => x.NumberOfSeats).ToList();
                }

                else if (sort == "Highest available seats")
                {
                    data = data.OrderByDescending(x => x.NumberOfSeats).ToList();
                }
                //else if (sort == "My favourites")
                //{
                //    data = data.Where(x => x.IsFavourite == true).ToList();

                    
                //}
                else if (data.Count == 0)
                {
                    return PartialView("_NoMissionPartial");

                }

                else if (sort == "Registration deadline")
                {
                    data = data.OrderBy(x => x.EndDate).ToList();

                }
                else if(sort == "Top Themes")
                {
                    var groupeddata = data.GroupBy(theme => theme.MissionThemes).ToList();
                    var sortdata = groupeddata.OrderByDescending(data=> data.Count()).ToList();
                    data = sortdata.SelectMany(group => group).ToList();
                }
                else if(sort == "Most Ranked")
                {
                    data = data.OrderByDescending(data=>data.Ratings).ToList();
                }
                else if(sort == "Top Favourite")
                {
                    data = data.OrderByDescending(data=>data.MostFavourite).ToList();
                }
                else if(sort == "Random")
                {
                    Random random = new Random();
                    data = data.OrderByDescending(data=>random.Next()).ToList();
                }
            }

            if (SearchText != null)
            {
                SearchText = SearchText.ToLower();

                data = data.Where(x => x.Title.ToLower().Contains(SearchText) || x.ShortDescription.ToLower().Contains(SearchText)).ToList();
                if (data.Count == 0)
                {
                    return PartialView("NoMissionPartial");
                }

            }


            return PartialView("_FilterPagePartial", data);
        }
        #endregion

        //volunteer mission page
        public IActionResult VolunteeringMissionPage(long? id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["missionid"] = (Int32)id;
                return RedirectToAction("Index", "Home");
            }
            if (HttpContext.Session.GetString("FirstName") != null)
            {
                var missionid = _db.Missions.FirstOrDefault(x => x.MissionId == id);
                var email = HttpContext.Session.GetString("Email");
                ViewBag.missionid = id;
                string user = HttpContext.Session.GetString("UserId");
                long userid = long.Parse(user);
                ViewBag.UserId = userid;
                var mission = _volunteerMissionCard.GetMission(id, userid);

                return View(mission);
            }
            return RedirectToAction("Index", "Home");
        }

        //Add to favourite
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
                var applyValue = 0;
                ViewBag.ApplyValue = applyValue;
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


        //Comments
        [HttpPost]
        public JsonResult DisplayComments(long? id, long UserId, string CommentText)
        {
            var comments = _db.Comments.ToList();
            var commentsDisplay = comments.Where(t => t.MissionId == id && t.DeletedAt == null).ToList();
            Comment comment = new Comment();
            comment.MissionId = (long)id;
            comment.UserId = UserId;
            comment.CommentText = CommentText;
            _db.Comments.Add(comment);
            _db.SaveChanges();
            
            return new JsonResult(commentsDisplay);
        }


        [HttpPost]
        public void ApplyToMission(long? missionid)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            Mission? checkmission = _db.Missions.FirstOrDefault(mission => mission.MissionId == missionid && mission.DeletedAt == null);
            if(checkmission != null)
            {
                MissionApplication? checkapplication = _db.MissionApplications.FirstOrDefault(application => application.MissionId == checkmission.MissionId && application.UserId == userid);
                if(checkapplication == null)
                {
                    MissionApplication newapplication = new MissionApplication();
                    newapplication.UserId = userid;
                    newapplication.MissionId = checkmission.MissionId;
                    newapplication.ApprovalStatus = "Pending";
                    newapplication.AppliedAt = DateTime.Now;
                    newapplication.CreatedAt = DateTime.Now;
                    _db.MissionApplications.Add(newapplication);
                    _db.SaveChanges();
                }
            }
            
        }



        [HttpPost]
        public bool RecommendToCoWorker(long missionid, List<string> selecteduser)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            User? currentuser = _db.Users.FirstOrDefault(user => user.UserId == userid);
            List<User>? usertomail = _db.Users.Where(seluser=>selecteduser.Contains(seluser.UserId.ToString())).ToList();
            if(usertomail != null)
            {
                foreach(User coworker in usertomail)
                {
                    _notification.AddRecommendNotification(userid, coworker.UserId, missionid, 2);
                    try
                    {
                        string email = coworker.Email;
                        var link = "<a href=\"https://localhost:5001/Mission/VolunteeringMissionPage/" + missionid + "\">Story Link</a>";
                        MailMessage newMail = new MailMessage();
                        SmtpClient client = new SmtpClient("smtp.gmail.com");
                        newMail.From = new MailAddress("ciplatform333@gmail.com", "CI Platform");
                        newMail.To.Add(email);
                        newMail.Subject = "Mission Recommended by " + currentuser.FirstName + " " + currentuser.LastName;
                        newMail.IsBodyHtml = true;
                        newMail.Body = currentuser.FirstName + " " + currentuser.LastName + "Recommended you the below Mission<br><br><br>" + link;
                        client.EnableSsl = true;
                        client.Port = 587;
                        client.Credentials = new System.Net.NetworkCredential("ciplatform333@gmail.com", "jbdxshjsnfhyimnp");
                        client.Send(newMail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return true;
            }
            return false;
        }
       

        [HttpPost]
        public void AddRating(long missionid, int ratingvalue)
        {
            string? user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            MissionRating? checkexist = _db.MissionRatings.FirstOrDefault(rating => rating.MissionId == missionid && rating.UserId == userid && rating.DeletedAt == null);
            if (checkexist != null)
            {
                checkexist.Rating = ratingvalue;
                _db.SaveChanges();

            }
            else
            {
                MissionRating newrating = new MissionRating();
                newrating.MissionId = missionid;
                newrating.UserId = userid;
                newrating.Rating = ratingvalue;
                _db.MissionRatings.Add(newrating);
                _db.SaveChanges();
            }
        }

    }
}
