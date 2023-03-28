using Microsoft.AspNetCore.Mvc;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;





namespace CI_Platform_Web.Controllers
{
    public class StoryController : Controller
    {

        private readonly CiPlatformDbContext _db;
        private readonly IStoryCardsRepository _storyCards;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StoryController(CiPlatformDbContext db, IStoryCardsRepository storyCards, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _storyCards = storyCards;
            _hostEnvironment = hostEnvironment;
        }



        public IActionResult StoriesListing()
        {
            var storylist = _storyCards.GetStories();
            return View(storylist);

            
        }

        public IActionResult ShareYourStory()
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var usermissions = _storyCards.GetUserMissions(userid);
            return View(usermissions);
        }


        [HttpPost]
        public IActionResult Submit(long missionId, string StoryTitle, DateTime Date, string StoryDescription)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var submitcondition = _db.Stories.Where(x => x.MissionId == missionId && x.UserId == (long)userid && x.Status=="DRAFT").FirstOrDefault();
            if (submitcondition != null)
            {

                //Story story = new Story();
                //story.MissionId = missionId;
                //story.Status = "PENDING";
                //story.Title = StoryTitle;
                //story.Description = StoryDescription;
                //story.CreatedAt = Date;
                //_db.Stories.Add(story);
                submitcondition.Status = "PENDING";
                submitcondition.Title = StoryTitle;
                submitcondition.Description = StoryDescription;
                submitcondition.CreatedAt = Date;
                _db.Stories.Update(submitcondition);
                _db.SaveChanges();
                return RedirectToAction("ShareYourStory");
            }
            return View();
           
        }

        public JsonResult DraftedData(long missionid)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var saveddata = _db.Stories.FirstOrDefault(x=> x.MissionId == missionid && x.UserId== userid);
            return new JsonResult(saveddata);


        }


        [HttpPost]
        public IActionResult ShareYourStory(ShareStoryVm obj, List<IFormFile> storyimg)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var savecondition = _db.Stories.Where(x => x.MissionId == obj.MissionId && (x.UserId == (long)userid)).FirstOrDefault();
            if(savecondition == null)
            {
                Story story = new Story();
               
                story.MissionId = obj.MissionId;
                story.UserId = userid;
                story.Status = "DRAFT";
                story.Title = obj.StoryTitle;
                story.Description = obj.StoryDesctiption;
                story.CreatedAt = obj.Date;
                _db.Stories.Add(story);
                _db.SaveChanges(true);
                


                //for images input

                //try
                //{
                //    if(storyimg.Count > 0)
                //    {
                //        foreach(var img in storyimg)
                //        {
                //            string filename = img.FileName;
                //            filename = Path.GetFileName(filename);
                //            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//storyImages");

                //            var stream = new FileStream(uploadpath, FileMode.Create);
                //            img.CopyToAsync(stream);
                //        }
                //        ViewBag.Message = "Total " + storyimg.Count.ToString() + "Images Uploaded Sucessfully";
                //    }
                //}
                //catch (Exception ex)
                //{

                //}

                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (storyimg != null)
                {
                    foreach (var img in storyimg)
                    {
                        StoryMedium storyMedia = new StoryMedium();
                        storyMedia.StoryId = story.StoryId;
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"assets/storyImages");
                        var extension = Path.GetExtension(img.FileName);

                        using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            img.CopyTo(filestream);
                        }
                        storyMedia.Type = extension;
                        storyMedia.Path = @"assets/storyImages" + fileName + extension;
                        _db.Add(storyMedia);
                        _db.SaveChanges(true);
                    }
                }


                //for images input





                obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
                obj.StoryTitle = null;
                return View(obj);
            }
            else if(savecondition.Status == "DRAFT")
            {
                ViewBag.DraftedStory = savecondition;
                
                
                savecondition.Title = obj.StoryTitle;
                savecondition.Description = obj.StoryDesctiption;
                savecondition.CreatedAt = obj.Date;
                _db.Stories.Update(savecondition);
                _db.SaveChanges(true);
                obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
                obj.StoryTitle = null;
                return View(obj);
            }
            obj.UserAppliedMissions = _storyCards.GetUserMissions(userid).UserAppliedMissions;
            obj.StoryTitle = null;
            
            return View(obj);
            
            
        }

       
    }
}
