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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly CiPlatformDbContext _db;

        public UserProfileRepository(CiPlatformDbContext db)
        {
            _db = db;
        }


        public UserProfileVm GetUserDetails(long userid)
        {
            var currentuser = _db.Users.FirstOrDefault(x => x.UserId == userid);

            if (currentuser != null)
            {
                UserProfileVm userdetails = new UserProfileVm();
                userdetails.Name = currentuser.FirstName;
                userdetails.Surname = currentuser.LastName;
                userdetails.WhyIVolunteer = currentuser.WhyIVolunteer;
                userdetails.Department = currentuser.Department;
                userdetails.EmployeeId = currentuser.EmployeeId;
                userdetails.MyProfile = currentuser.ProfileText;
                userdetails.Title = currentuser.Title;
                userdetails.LinkedIn = currentuser.LinkedInUrl;
                userdetails.countries = _db.Countries.ToList();
                userdetails.CountryId = currentuser.CountryId;
                userdetails.cities = _db.Cities.Where(x => x.CountryId == userdetails.CountryId).ToList();
                userdetails.CityId = currentuser.CityId;
                userdetails.Allskills = _db.Skills.ToList();
                userdetails.Avatar = currentuser.Avatar;
                var userskills = _db.UserSkills.Where(x=>x.UserId==userid).ToList();
                //var example = _db.UserSkills.Include(m=>m.
                List<UserSkillsVm> userskillslist = new List<UserSkillsVm>();
                foreach(var user in userskills)
                {
                    UserSkillsVm skill = new UserSkillsVm();
                    skill.SkillId = user.SkillId;
                    var skillname = _db.Skills.FirstOrDefault(x=>x.SkillId==skill.SkillId);
                    skill.Skillname = skillname.SkillName;
                    skill.UserSkillId = user.UserSkillId;
                    userskillslist.Add(skill);
                }
                userdetails.UserSkills = userskillslist;
                return userdetails;
            }
            return null;
        }


        //public UserProfileVm UpdateUser(UserProfileVm obj, long userid)
        //{
            
        //        var userupdate = _db.Users.FirstOrDefault(x => x.UserId == userid);
        //        userupdate.FirstName = obj.Name;
        //        userupdate.LastName = obj.Surname;
        //        userupdate.WhyIVolunteer = obj.WhyIVolunteer;
        //        userupdate.Department = obj.Department;
        //        userupdate.EmployeeId = obj.EmployeeId;
        //        userupdate.ProfileText = obj.MyProfile;
        //        userupdate.Title = obj.Title;
        //        userupdate.LinkedInUrl = obj.LinkedIn;
        //        userupdate.CountryId = (long)obj.CountryId;
        //        userupdate.CityId = (long)obj.CityId;
        //        _db.Users.Update(userupdate);
        //        _db.SaveChanges();

        //    return 
        //}



    }
}
