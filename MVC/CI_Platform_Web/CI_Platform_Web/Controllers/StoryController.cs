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
            return View();
        }
    }
}
