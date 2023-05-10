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
            IEnumerable<User> usersdetails = _db.Users.Where(x=>x.DeletedAt == null).ToList();
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

        public AdminUserCreateVm Getcountry()
        {
            AdminUserCreateVm vm = new AdminUserCreateVm();
            List<Country> countries = _db.Countries.ToList();
            List<Country> fillcountries = new List<Country>();
            foreach(Country country in countries)
            {
                Country singlecountry = new Country();
                singlecountry.CountryId = country.CountryId;
                singlecountry.Name = country.Name;
                fillcountries.Add(singlecountry);
            }
            vm.Countries = fillcountries;
            return vm;
        }



        public List<City> GetCities(long countryid)
        {
            List<City> citys = _db.Cities.Where(x => x.CountryId == countryid).ToList();
            List<City> cities = new List<City>();
            foreach (City obj in citys)
            {
                City city = new City();
                city.CityId = obj.CityId;
                city.CountryId = obj.CountryId;
                city.Name = obj.Name;
                cities.Add(city);
            }
            return cities;

        }




        public void AddUser(AdminUserCreateVm obj)
        {
            User user = new User();
            user.FirstName = obj.FirstName;
            user.LastName = obj.LastName;
            user.Avatar = obj.Avatar;
            user.Email = obj.Email;
            user.Department = obj.Department;
            user.Password = obj.Password;
            user.PhoneNumber = obj.PhoneNumber;
            user.EmployeeId = obj.EmployeeId;
            user.CityId = obj.CityId;
            user.CountryId = obj.CountryId;
            user.ProfileText = obj.ProfileText;
            user.Status = obj.Status;
            user.Role = obj.Role;
            _db.Users.Add(user);
            _db.SaveChanges();
            
        }



        public AdminUserCreateVm GetUser(long userid)
        {
            AdminUserCreateVm vm = new AdminUserCreateVm();
            User user = _db.Users.Find(userid);
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Avatar = user.Avatar;
            vm.PhoneNumber = user.PhoneNumber;
            vm.CityId = user.CityId;
            vm.Status = user.Status;
            vm.Email = user.Email;  
            vm.CountryId = user.CountryId;
            vm.Department = user.Department;
            vm.EmployeeId = user.EmployeeId;
            vm.ProfileText = user.ProfileText;
            vm.Password = user.Password;
            vm.Role = user.Role;
            List<Country> countries = _db.Countries.ToList();
            List<Country> fillcountries = new List<Country>();
            foreach (Country country in countries)
            {
                Country singlecountry = new Country();
                singlecountry.CountryId = country.CountryId;
                singlecountry.Name = country.Name;
                fillcountries.Add(singlecountry);
            }
            vm.Countries = fillcountries;
            return vm;
        }

        
        public void EditUser(AdminUserCreateVm obj)
        {
            User edituser = _db.Users.Find(obj.UserId);
            if (edituser != null)
            {
                edituser.FirstName = obj.FirstName;
                edituser.LastName = obj.LastName;
                edituser.CountryId = obj.CountryId;
                edituser.CityId = obj.CityId;
                edituser.Status = obj.Status;
                edituser.EmployeeId = obj.EmployeeId;
                edituser.ProfileText = obj.ProfileText;
                edituser.Department = obj.Department;
                edituser.PhoneNumber = obj.PhoneNumber;
                edituser.Role = obj.Role;
                if(obj.Avatar != null)
                {
                    edituser.Avatar = obj.Avatar;
                }
                
                _db.Users.Update(edituser);
                _db.SaveChanges();
            }
        }

        public string DeleteUser(long userid)
        {
            string reply = "";
            User? deleteuser = _db.Users.Find(userid);
            if (deleteuser != null)
            {
                deleteuser.DeletedAt = DateTime.Now;
                _db.Update(deleteuser);
                _db.SaveChanges();
                reply = "User Deleted Sucessfully";
                return reply;
            }
            return reply;

        }
        
    }
}
