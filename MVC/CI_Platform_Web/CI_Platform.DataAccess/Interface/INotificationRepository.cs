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
        public void AddMissionNotification(long missionid, long adminid);

        public List<NotificationVm> GetNotifications(long userid);

        public void AddRecommendNotification(long senderid, long receiverid, long id, int type);

        public void AddTimeSheetApprovalNotification(long timesheetid, long adminid);

        public void AddTimeSheetDeclineNotification(long timesheetid, long adminid);

        public void AddStoryStatusNotification(long storyid, long adminid, string status);

        public void AddMissionApplicationStatusNotification(long applicationid, long adminid, string status);
    }
}
