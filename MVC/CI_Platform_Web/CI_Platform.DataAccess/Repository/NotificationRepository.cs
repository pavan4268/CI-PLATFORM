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


        public void AddMissionNotification(long missionid)
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
                        newnotification.FromUserId = 3;
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


        



    }
}
