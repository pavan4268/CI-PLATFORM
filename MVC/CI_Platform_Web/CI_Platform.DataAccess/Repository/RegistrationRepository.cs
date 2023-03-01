using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class RegistrationRepository : IRegistrationRepository

    {
        private readonly CiPlatformDbContext _ciPlatformDbContext;

        public RegistrationRepository(CiPlatformDbContext ciPlatformDbContext)
        {
            _ciPlatformDbContext = ciPlatformDbContext;
        }
        //public void add(User user)
        //{
        //    _ciPlatformDbContext.Users.Add(user);
        //}
        //public void Save(User user) 
        //{
        //    _ciPlatformDbContext.SaveChanges();
        //}
    }
}
