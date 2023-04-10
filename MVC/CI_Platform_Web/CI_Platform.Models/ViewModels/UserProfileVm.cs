using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class UserProfileVm
    {
        [Required(ErrorMessage ="Name is Required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Surname is Required.")]
        public string? Surname { get; set; }

        public string? Email { get; set; }

        public string? EmployeeId { get; set; }

        public string? Manager { get; set; }

        public string? Title { get; set; }

        public string? Department { get; set; }

        [Required(ErrorMessage ="Profile Text is Required.")]
        public string? MyProfile { get; set; }

        public string? WhyIVolunteer { get; set; }

        public List<City> cities { get; set; }

        public long? CityId { get; set; }

        public List<Country> countries { get; set; }

        [Required(ErrorMessage ="Please Select a Country.")]
        public long? CountryId { get; set; }

        public string? Availability { get; set; }

        public string? LinkedIn { get; set; }

        public List<Skill> Allskills { get; set; }

        public List<UserSkillsVm> UserSkills { get; set; }

        public string? Avatar { get; set; }

        //[Required(ErrorMessage ="Please Enter Current Password.")]
        //public string? CurrentPassword { get; set; }

        //[Required(ErrorMessage ="Please Enter the New Password")]
        //public string? NewPassword { get; set; }

        //[Required(ErrorMessage ="Please enter confirm password")]
        //[Compare("NewPassword" , ErrorMessage ="New Password and Confirm Password must Match")]
        //public string? CnfPassword { get; set; }
    }
}
