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
    public class AdminTimeSheetRepository : IAdminTimeSheetRepository
    {
        private readonly CiPlatformDbContext _db;
        private readonly INotificationRepository _notification;


        public AdminTimeSheetRepository(CiPlatformDbContext db, INotificationRepository notification)
        {
            _db = db;
            _notification = notification;
        }

        public List<AdminTimesheetDisplayVm> GetTimeSheets()
        {
            List<AdminTimesheetDisplayVm> result = new List<AdminTimesheetDisplayVm>();
            List<Timesheet> timesheets = _db.Timesheets.Where(sheet=>sheet.Status == "PENDING" && sheet.DeletedAt == null).ToList();
            foreach(Timesheet data in timesheets)
            {
                Mission? fortitle = _db.Missions.FirstOrDefault(mission=>mission.MissionId == data.MissionId && mission.DeletedAt == null);
                User? forusername = _db.Users.FirstOrDefault(user=>user.UserId == data.UserId && user.DeletedAt == null);
                AdminTimesheetDisplayVm vm = new AdminTimesheetDisplayVm();
                vm.TimesheetId = data.TimesheetId;
                vm.MissionId = data.MissionId;
                vm.UserId = data.UserId;
                vm.Username = forusername?.FirstName + " " + forusername?.LastName;
                vm.Action = data.Action;
                vm.Time = data.Time;
                vm.MissionTitle = fortitle?.Title;
                vm.DateVolunteered = data.DateVolunteered.ToString("dd-MM-yyyy");

                result.Add(vm);
            }
            return result;
        }


        public string ApproveTimeSheet(long timesheetid, long adminid)
        {
            string? reply = string.Empty;
            Timesheet? approvesheet = _db.Timesheets.FirstOrDefault(timesheet => timesheet.TimesheetId == timesheetid && timesheet.DeletedAt == null);
            if (approvesheet != null)
            {
                approvesheet.Status = "APPROVED";
                approvesheet.UpdatedAt = DateTime.Now;
                _db.Timesheets.Update(approvesheet);
                _notification.AddTimeSheetApprovalNotification(approvesheet.TimesheetId, adminid);
                _db.SaveChanges();
                reply = "Approved TimeSheet";
                return reply;
            }
            return reply;
        }

        public string DeclineTimeSheet(long timesheetid, long adminid)
        {
            string? reply = string.Empty;
            Timesheet? declinesheet = _db.Timesheets.FirstOrDefault(timesheet=>timesheet.TimesheetId==timesheetid && timesheet.DeletedAt == null);
            if(declinesheet != null)
            {
                declinesheet.Status = "DECLINED";
                declinesheet.UpdatedAt = DateTime.Now;
                _db.Timesheets.Update(declinesheet);
                _notification.AddTimeSheetDeclineNotification(declinesheet.TimesheetId,adminid);
                _db.SaveChanges();
                reply = "Timesheet Declined";
                return reply;
            }
            return reply;
        }
    }
}
