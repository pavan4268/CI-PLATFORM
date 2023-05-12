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
    public class NotificationRepository : INotificationRepository
    {
        private readonly CiPlatformDbContext _db;

        public NotificationRepository(CiPlatformDbContext db)
        {
            _db = db;
        }



        public List<NotificationVm> GetNotifications(long userid)
        {
            List<NotificationVm> response = new List<NotificationVm>();
            List<Notification> list = _db.Notifications.Where(notification => notification.ToUserId == userid).ToList();
            if (list.Any())
            {
                foreach (Notification vm in list)
                {
                    NotificationVm notification = new NotificationVm();
                    notification.Status = vm.Status;
                    notification.Message = vm.Message;
                    notification.Url = vm.Url;
                    notification.CreatedAt = vm.CreatedAt;
                    notification.NotifId = vm.NotifId;
                    notification.NotifTypeId = vm.NotifTypeId;
                    if(notification.NotifTypeId == 2 || notification.NotifTypeId == 3)
                    {
                        User? foravataar = _db.Users.FirstOrDefault(user => user.UserId == vm.FromUserId);
                        notification.FromUserAvtaar = foravataar?.Avatar;
                    }
                    response.Add(notification);
                }
            }
            return response;
        }


        public void AddMissionNotification(long missionid, long adminid)
        {
            
            Mission? findmission = _db.Missions.FirstOrDefault(mission=>mission.MissionId == missionid && mission.DeletedAt == null);
            if(findmission != null)
            {
                List<User>? findusers = _db.Users.Where(user=>user.CountryId == findmission.CountryId && user.DeletedAt == null).ToList();
                if (findusers.Any())
                {
                    foreach (User notifyuser in findusers)
                    {
                        Notification newnotification = new Notification();
                        newnotification.FromUserId = adminid;
                        newnotification.ToUserId = notifyuser.UserId;
                        newnotification.Message = "New Mission - " + findmission.Title;
                        newnotification.Url = "https://localhost:5001/Mission/VolunteeringMissionPage/" + findmission.MissionId;
                        newnotification.NotifTypeId = 1;
                        _db.Notifications.Add(newnotification);
                        _db.SaveChanges();
                    }

                }
            }


        }


        public void AddRecommendNotification(long senderid, long receiverid, long id, int type)
        {
            User? sender = _db.Users.FirstOrDefault(user => user.UserId == senderid && user.DeletedAt == null);
            
            if(type == 2)
            {
                Mission? findmission = _db.Missions.FirstOrDefault(mission => mission.MissionId == id && mission.DeletedAt == null);
                Notification newnotification = new Notification();
                newnotification.FromUserId = senderid;
                newnotification.ToUserId = receiverid;
                newnotification.Message = sender?.FirstName + " " + sender?.LastName + " - Recommends The Mission" + findmission?.Title;
                newnotification.Url = "https://localhost:5001/Mission/VolunteeringMissionPage/" + findmission?.MissionId;
                newnotification.NotifTypeId = type;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
            else
            {
                Story? findstory = _db.Stories.FirstOrDefault(story => story.StoryId == id && story.DeletedAt == null);
                Notification newnotification = new Notification();
                newnotification.FromUserId = senderid;
                newnotification.ToUserId = receiverid;
                newnotification.Message = sender?.FirstName + " " + sender?.LastName + " - Recommends The Story" + findstory?.Title;
                newnotification.Url = "https://localhost:5001/Story/StoryDetails/" + findstory?.StoryId;
                newnotification.NotifTypeId = type;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
        }


        public void AddTimeSheetApprovalNotification(long timesheetid, long adminid)
        {
            Timesheet? findsheet = _db.Timesheets.FirstOrDefault(sheet => sheet.TimesheetId == timesheetid && sheet.DeletedAt == null);
            Mission? findtitle = _db.Missions.FirstOrDefault(mission => mission.MissionId==findsheet.MissionId);
            if(findsheet != null)
            {
                Notification newnotification = new Notification();
                newnotification.ToUserId = (long)findsheet.UserId;
                if(findsheet.Action == null)
                {
                    newnotification.Message = "Volunteering Timesheet Request for Time-Based Mission - " + findtitle?.Title + " Is Approved";
                    newnotification.NotifTypeId = 4;
                }
                else
                {
                    newnotification.Message = "Volunteering Timesheet Request for Goal-Based Mission - " + findtitle?.Title + " Is Approved";
                    newnotification.NotifTypeId = 5;
                }
                newnotification.Url = "https://localhost:5001/User/VolunteeringTimesheet";
                newnotification.FromUserId = adminid;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
        }



        public void AddTimeSheetDeclineNotification(long timesheetid, long adminid)
        {
            Timesheet? findsheet = _db.Timesheets.FirstOrDefault(sheet => sheet.TimesheetId == timesheetid && sheet.DeletedAt == null);
            Mission? findtitle = _db.Missions.FirstOrDefault(mission => mission.MissionId == findsheet.MissionId);
            if (findsheet != null)
            {
                Notification newnotification = new Notification();
                newnotification.ToUserId = (long)findsheet.UserId;
                if (findsheet.Action == null)
                {
                    newnotification.Message = "Volunteering Timesheet Request for Time-Based Mission - " + findtitle?.Title + " Is Declined";
                    newnotification.NotifTypeId = 4;
                }
                else
                {
                    newnotification.Message = "Volunteering Timesheet Request for Goal-Based Mission - " + findtitle?.Title + " Is Declined";
                    newnotification.NotifTypeId = 5;
                }
                newnotification.Url = "https://localhost:5001/User/VolunteeringTimesheet";
                newnotification.FromUserId = adminid;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
        }

        public void AddStoryStatusNotification(long storyid, long adminid, string status)
        {
            Story? findstory = _db.Stories.FirstOrDefault(st => st.StoryId == storyid && st.DeletedAt == null);
            Mission? findtitle = _db.Missions.FirstOrDefault(mission => mission.MissionId == findstory.MissionId && mission.DeletedAt == null);
            if (findstory != null)
            {
                Notification newnotification = new Notification();
                newnotification.ToUserId = findstory.UserId;
                newnotification.NotifTypeId = 6;
                newnotification.Message = "Story for Mission - " + findstory?.Title + " Has Been " + status;
                newnotification.Url = "https://localhost:5001/Story/StoryDetails?storyid=" + findstory.StoryId;
                newnotification.FromUserId = adminid;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
        }



        public void AddMissionApplicationStatusNotification(long applicationid, long adminid, string status)
        {
            MissionApplication? findapplication = _db.MissionApplications.FirstOrDefault(application=>application.MissionApplicationId==applicationid && application.DeletedAt == null);
            User? finduser = _db.Users.FirstOrDefault(user => user.UserId == findapplication.UserId && user.DeletedAt == null);
            Mission? findtitle = _db.Missions.FirstOrDefault(mission=> mission.MissionId == findapplication.MissionId && mission.DeletedAt == null);
            if(findapplication != null)
            {
                Notification newnotification = new Notification();
                newnotification.ToUserId = findapplication.UserId;
                newnotification.FromUserId = adminid;
                newnotification.NotifTypeId = 7;
                newnotification.Message = "Mission Application for the Mission - " + findtitle.Title + " Has Been " + status;
                newnotification.Url = "https://localhost:5001/Mission/VolunteeringMissionPage/" + findtitle.MissionId;
                _db.Notifications.Add(newnotification);
                _db.SaveChanges();
            }
        }

    }
}
