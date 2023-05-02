using CI_Platform.Entities.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionCreateVm
    {

        
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public List<long>? SkillIds { get; set; }





        [Required(ErrorMessage = "Mission Title is Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Special Characters not Allowed")]
        public string Title { get; set; } = null!;


        [Required(ErrorMessage = "Short Description is Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Special Characters not Allowed")]
        public string? ShortDescription { get; set; }


        [Required(ErrorMessage = "Description is Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Special Characters not Allowed")]
        public string? Description { get; set; }



        [Required(ErrorMessage ="Please Select a StartDate")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage ="Please Select an EndDate")]
        public DateTime? EndDate { get; set; }

        public DateTime? DeadLine { get; set; }





        [Required(ErrorMessage = "Mission Type is Required.")]
        public string MissionType { get; set; } = null!;





        [Required(ErrorMessage = "Organization Name is Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Special Characters not Allowed")]
        public string? OrganizationName { get; set; }


        [Required(ErrorMessage = "Organization Details are Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Special Characters not Allowed")]
        public string? OrganizationDetails { get; set; }





        [Required(ErrorMessage = "Availability is Required.")]
        public string? Availability { get; set; }

        public int? TotalSeats { get; set; }




        public List<Country>? Countries { get; set; }

        public List<MissionTheme>? MissionThemes { get; set; }

        public List<Skill>? SkillList { get; set; }





        //[Required(ErrorMessage = "Goal Value is Required.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage ="Special Characters not Allowed")]
        public string? GoalObjective { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Goal Value must be a positive integer.")]
        public int? GoalValue { get; set; }

        public List<IFormFile>? Images { get; set; }

        public string? VideoURL { get; set; }

        public List<string>? Imagepaths { get; set; }

        public List<string>? Documentpaths { get; set; }
    }
}
