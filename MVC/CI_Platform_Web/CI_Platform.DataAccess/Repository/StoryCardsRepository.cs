using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Repository.Repository
{
    public class StoryCardsRepository : IStoryCardsRepository
    {
        private readonly CiPlatformDbContext _db;

        public StoryCardsRepository(CiPlatformDbContext db)
        {
            _db = db;
        }

        public List<StoryListingVm> GetStories()
        {
            List<StoryListingVm> stories = new List<StoryListingVm>();
            var storylist = _db.Stories.Where(x=>x.Status == "PUBLISHED").ToList();
            foreach(var story in storylist)
            {
                var user = _db.Users.Where(x=>x.UserId == story.UserId).FirstOrDefault();
                var mission = _db.Missions.Where(x=>x.MissionId == story.MissionId).FirstOrDefault();
                var theme = _db.MissionThemes.Where(x=>x.MissionThemeId==mission.ThemeId).FirstOrDefault();
                StoryListingVm storycard = new StoryListingVm();
                storycard.StoryTitle = story.Title;
                storycard.StoryDescription = story.Description;
                storycard.UserName = user.FirstName + " " + user.LastName;
                storycard.MissionTheme = theme.Title;
                storycard.PublishedAt = (DateTime)story.PublishedAt;
                stories.Add(storycard);
            }
            
            
            
            
            return stories;
        }
    }
}
