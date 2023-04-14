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
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly CiPlatformDbContext _db;

        public AdminUserRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public List<AdminUserDisplayVm> GetUsers()
        {
            List<AdminUserDisplayVm> users = new List<AdminUserDisplayVm>();
            IEnumerable<User> usersdetails = _db.Users.ToList();
            foreach(User user in usersdetails)
            {
                AdminUserDisplayVm display = new AdminUserDisplayVm();
                display.Status = user.Status;
                display.Email = user.Email;
                display.Department = user.Department;
                display.UserId = user.UserId;
                display.EmployeeId = user.EmployeeId;
                display.FirstName = user.FirstName;
                display.LastName = user.LastName;
                users.Add(display);
            }
            return users;
        }




    }
}
