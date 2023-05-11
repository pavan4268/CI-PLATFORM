using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface INotificationRepository
    {
        public void AddMissionNotification(long missionid);

        public List<NotificationVm> GetNotifications(long userid);

        public void AddRecommendNotification(long senderid, long receiverid, long id, int type);
    }
}
