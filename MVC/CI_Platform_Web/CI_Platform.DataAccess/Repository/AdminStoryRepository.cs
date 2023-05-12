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
    public class AdminStoryRepository:IAdminStoryRepository
    {

        private readonly CiPlatformDbContext _db;
        private readonly INotificationRepository _notification;

        public AdminStoryRepository(CiPlatformDbContext db, INotificationRepository notification)
        {
            _db = db;
            _notification = notification;
        }


        public List<AdminStoryDisplayVm> GetStories()
        {
            List<AdminStoryDisplayVm> stories = new List<AdminStoryDisplayVm>();
            List<Story> pendingstories = _db.Stories.Where(story=>story.Status=="PENDING" && story.DeletedAt==null).ToList();
            foreach (var story in pendingstories)
            {
                Mission? fortitle = _db.Missions.FirstOrDefault(mission => mission.MissionId == story.MissionId);
                User? forusername = _db.Users.FirstOrDefault(user => user.UserId == story.UserId);
                AdminStoryDisplayVm vm = new AdminStoryDisplayVm();
                vm.Title = story.Title;
                vm.MissionTitle = fortitle?.Title;
                vm.MissionId = story.MissionId;
                vm.UserId = story.UserId;
                vm.UserName = forusername?.FirstName + " " + forusername?.LastName;
                vm.StoryId = story.StoryId;
                stories.Add(vm);
            }
            return stories;
        }



        public AdminStoryDetailsVm GetDetails(long storyid)
        {
            AdminStoryDetailsVm vm = new AdminStoryDetailsVm();
            Story? getstory = _db.Stories.FirstOrDefault(story=>story.StoryId==storyid);
            vm.StoryId = getstory.StoryId;
            vm.StoryDescrription = getstory.Description;
            vm.Title = getstory.Title;
            Mission? fortitle = _db.Missions.FirstOrDefault(mission=>mission.MissionId==getstory.MissionId);
            User? forusername = _db.Users.FirstOrDefault(user => user.UserId == getstory.UserId);
            vm.UserName = forusername?.FirstName + " " + forusername?.LastName;
            vm.UserImage = forusername?.Avatar;
            vm.MissionTitle= fortitle?.Title;
            List<string> storyimages = new List<string>();
            List<string> storyvideos = new List<string>();
            List<StoryMedium> getmedia = _db.StoryMedia.Where(story => story.StoryId == getstory.StoryId).ToList();
            foreach(var media in getmedia)
            {
                if(media.Type == "video")
                {
                    string videopath = media.Path;
                    storyvideos.Add(videopath);
                }
                else
                {
                    string imagepath = media.Path;
                    storyimages.Add(imagepath);
                }

            }
            vm.ImagePaths = storyimages;
            vm.VideoURLs = storyvideos;
            return vm;
        }

        public string StoryApprove(long storyid, long adminid)
        {
            string? reply = "";
            Story? approvestory = _db.Stories.FirstOrDefault(story=>story.StoryId == storyid);
            if(approvestory != null)
            {
                approvestory.Status = "PUBLISHED";
                approvestory.UpdatedAt = DateTime.Now;
                approvestory.PublishedAt = DateTime.Now;
                _db.Stories.Update(approvestory);
                _db.SaveChanges();
                _notification.AddStoryStatusNotification(storyid, adminid, "Published");
                return reply;
            }
            reply = "Unable to Approve Story";
            return reply;
        }

        public string StoryDecline(long storyid, long adminid)
        {
            string? reply = "";
            Story? declinestory = _db.Stories.FirstOrDefault(story=>story.StoryId==storyid);
            if(declinestory != null)
            {
                declinestory.Status = "DECLINED";
                declinestory.UpdatedAt = DateTime.Now;
                _db.Stories.Update(declinestory);
                _db.SaveChanges();
                _notification.AddStoryStatusNotification(storyid, adminid, "Declined");
                return reply;

            }
            reply = "Unable to Decline Story";
            return reply;
        }

        public string StoryDelete(long storyid)
        {
            string? reply = "";
            Story? deletestory = _db.Stories.FirstOrDefault(story=>story.StoryId==storyid);
            if(deletestory != null)
            {
                deletestory.DeletedAt = DateTime.Now;
                _db.Stories.Update(deletestory);
                _db.SaveChanges();
                return reply;

            }
            reply = "Unable to Delete Story";
            return reply;
        }





    }

}
