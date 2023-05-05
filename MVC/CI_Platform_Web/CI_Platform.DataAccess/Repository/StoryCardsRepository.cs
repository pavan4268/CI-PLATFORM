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
                storycard.Storyid = story.StoryId;
                stories.Add(storycard);
            }

            
            
            
            
            
            return stories;
        }

        public ShareStoryVm GetUserMissions(long userid)
        {
            
            ShareStoryVm shareStoryVm = new ShareStoryVm();
            List<StoryMissionListVm> userMissions = new List<StoryMissionListVm>();
            var appliedmissions = _db.MissionApplications.Where(x=>x.UserId == userid).ToList();
            foreach (var appliedmission in appliedmissions)
            {
                var mission = _db.Missions.Where(x=> x.MissionId==appliedmission.MissionId).FirstOrDefault();
                StoryMissionListVm item = new StoryMissionListVm();
                item.MissionId = appliedmission.MissionId;
                item.MissionName = mission.Title;
                userMissions.Add(item);

            }
            shareStoryVm.UserAppliedMissions = userMissions;
            //shareStoryVm.Date = DateTime.Now;
            return shareStoryVm;
        }

        public StoryDetailsVm GetStoryDetails(long stroyid)
        {
            StoryDetailsVm details = new StoryDetailsVm();
            
            var story = _db.Stories.Where(x=> x.StoryId == stroyid).FirstOrDefault();
            var user = _db.Users.FirstOrDefault(x => x.UserId == story.UserId);
            var username = user.FirstName + " " + user.LastName;
            details.Username = username;
            details.WhyIVolunteer = user.WhyIVolunteer;
            details.StoryDescription = story.Description;
            details.StoryTitle = story.Title;
            details.MissionId = story.MissionId;
            var users = _db.Users.Where(user=>user.DeletedAt==null).ToList();
            details.users = users;
            details.StoryId = stroyid;
            //story.views = story.views + 1;
            //_db.Stories.Update(story);
            return details;
        }

    }
}
