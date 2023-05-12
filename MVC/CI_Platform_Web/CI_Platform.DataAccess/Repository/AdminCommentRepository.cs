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
    public class AdminCommentRepository : IAdminCommentRepository
    {
        private readonly CiPlatformDbContext _db;

        public AdminCommentRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminCommentDisplayVm> GetComments()
        {
            List<AdminCommentDisplayVm> result = new List<AdminCommentDisplayVm>();
            List<Comment>? comments = _db.Comments.Where(comment=>comment.DeletedAt == null).ToList();
            foreach (Comment comment in comments)
            {
                Mission? findtitle = _db.Missions.FirstOrDefault(mission=>mission.MissionId == comment.MissionId);
                User? findname = _db.Users.FirstOrDefault(user=>user.UserId == comment.UserId);
                AdminCommentDisplayVm vm = new AdminCommentDisplayVm();
                vm.UserId = comment.UserId;
                vm.CommentId = comment.CommentId;
                vm.CommentText = comment.CommentText;
                vm.MissionId = comment.MissionId;
                vm.MissionTitle = findtitle.Title;
                vm.Username = findname.FirstName + " " + findname.LastName;
                result.Add(vm);
            }
            return result;
        }

    }
}
