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

        public StoryController(CiPlatformDbContext db, IStoryCardsRepository storyCards)
        {
            _db = db;
            _storyCards = storyCards;
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

        [HttpPost]
        public IActionResult Save(long missionId, string StoryTitle, DateTime Date, string StoryDescription)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var savecondition = _db.Stories.Where(x => x.MissionId == missionId && (x.UserId == (long)userid)).FirstOrDefault();
            if(savecondition == null)
            {
                Story story = new Story();
                story.MissionId = missionId;
                story.Status = "DRAFT";
                story.Title = StoryTitle;
                story.Description = StoryDescription;
                story.CreatedAt = Date;
                _db.Stories.Add(story);
                _db.SaveChanges();
                return View(story);
            }
            else if(savecondition.Status == "DRAFT")
            {
                ViewBag.DraftedStory = savecondition;
                savecondition.MissionId = missionId;
                savecondition.Status = "DRAFT";
                savecondition.Title = StoryTitle;
                savecondition.Description = StoryDescription;
                savecondition.CreatedAt = Date;
                _db.Stories.Update(savecondition);
                _db.SaveChanges(true);
                return View(savecondition);
            }
            return View(savecondition);

            
        }

        public JsonResult SavedData(long missionId)
        {
            string user = HttpContext.Session.GetString("UserId");
            long userid = long.Parse(user);
            var savedstory = _db.Stories.FirstOrDefault(x=>x.MissionId == missionId && x.UserId==userid);
            return new JsonResult(savedstory);
        }
    }
}
