using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IUserProfileRepository
    {
        public UserProfileVm GetUserDetails(long userid);

        //public UserProfileVm UpdateUser(UserProfileVm obj, long userid);
    }
}
