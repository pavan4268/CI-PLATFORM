using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Entities.Data;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly CiPlatformDbContext _ciPlatformDBContext;
        
            public LoginRepository(CiPlatformDbContext ciPlatformDBContext)
            {
             _ciPlatformDBContext = ciPlatformDBContext;
            }
        public async Task<User> AuthenticateUser(string email, string password)
        {
            var succeeded = await _ciPlatformDBContext.Users.FirstOrDefaultAsync(authUser => authUser.Email == email && authUser.Password == password);
            return succeeded;
        }

        public async Task<IEnumerable<User>> getuser()
        {
            return await _ciPlatformDBContext.Users.ToListAsync();
        }

        //Task<User> ILoginRepository.AuthenticateUser(string email, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IEnumerable<User>> ILoginRepository.getuser()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
