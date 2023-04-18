using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModels;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminUserRepository
    {
        public List<AdminUserDisplayVm> GetUsers();

        public AdminUserCreateVm Getcountry();

        public void AddUser(AdminUserCreateVm obj);

        public List<City> GetCities(long countryid);

        public AdminUserCreateVm GetUser(long userid);

        public void EditUser(AdminUserCreateVm obj);

        public string DeleteUser(long userid);
    }
}
