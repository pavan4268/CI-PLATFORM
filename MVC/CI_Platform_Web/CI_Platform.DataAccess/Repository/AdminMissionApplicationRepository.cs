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
    public class AdminMissionApplicationRepository: IAdminMissionApplicationRepository
    {
        private readonly CiPlatformDbContext _db;
        private readonly INotificationRepository _notification;

        public AdminMissionApplicationRepository(CiPlatformDbContext db, INotificationRepository notification)
        {
            _db = db;
            _notification = notification;
        }

        public List<AdminMissionApplicationDisplayVm> GetMissionApplications()
        {
            List<AdminMissionApplicationDisplayVm> vmList = new List<AdminMissionApplicationDisplayVm>();
            List<MissionApplication> missionApplications = _db.MissionApplications.Where(x=>x.ApprovalStatus=="Pending").ToList();
            foreach(MissionApplication missionApplication in missionApplications)
            {
                Mission? mission = _db.Missions.FirstOrDefault(x=>x.MissionId == missionApplication.MissionId);
                User? user = _db.Users.FirstOrDefault(x=>x.UserId == missionApplication.UserId);
                AdminMissionApplicationDisplayVm vm = new AdminMissionApplicationDisplayVm();
                vm.AppliedAt = missionApplication.AppliedAt.ToString("dd-MM-yyyy");
                vm.MissionName = mission.Title;
                vm.UserName = user.FirstName+" "+user.LastName;
                vm.MissionId = missionApplication.MissionId;
                vm.UserId = missionApplication.UserId;
                vm.MissionApplicationId = missionApplication.MissionApplicationId;
                vmList.Add(vm);
            }
            return vmList;
        }


        public string ApproveApplication(long applicationid, long adminid)
        {
            string? reply = null;
            MissionApplication? approveuser = _db.MissionApplications.FirstOrDefault(x=>x.MissionApplicationId == applicationid);
            if(approveuser != null)
            {
                approveuser.ApprovalStatus = "Approve";
                approveuser.UpdatedAt = DateTime.Now;
                _db.MissionApplications.Update(approveuser);
                _db.SaveChanges();
                _notification.AddMissionApplicationStatusNotification(approveuser.MissionApplicationId, adminid, "Approved");
                reply = "Application Approved";
                return reply;
            }
            return reply;
        }


        public string DeclineApplication(long applicationid, long adminid)
        {
            string? reply = null;
            MissionApplication? declineuser = _db.MissionApplications.FirstOrDefault(x=>x.MissionApplicationId == applicationid);
            if(declineuser != null)
            {
                declineuser.ApprovalStatus = "Decline";
                declineuser.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                _notification.AddMissionApplicationStatusNotification(declineuser.MissionApplicationId, adminid, "Declined");
                reply = "Application Declined";
                return reply;
            }
            return reply;
        }
    }
}
