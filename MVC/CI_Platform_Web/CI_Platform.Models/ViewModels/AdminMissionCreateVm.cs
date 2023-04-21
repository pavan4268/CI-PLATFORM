using CI_Platform.Entities.Data;
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

        [Required(ErrorMessage = "Mission Title is Required.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please enter only alphabetic characters and spaces")]
        public string Title { get; set; } = null!;


        [Required(ErrorMessage = "Short Description is Required.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please enter only alphabetic characters and spaces")]
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        [Required(ErrorMessage = "Mission Type is Required.")]
        public string MissionType { get; set; } = null!;


        [Required(ErrorMessage = "Organization Name is Required.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please enter only alphabetic characters and spaces")]
        public string? OrganizationName { get; set; }


        [Required(ErrorMessage = "Organization Details are Required.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please enter only alphabetic characters and spaces")]
        public string? OrganizationDetails { get; set; }


        [Required(ErrorMessage = "Availability is Required.")]
        public string? Availability { get; set; }

       

        public int? TotalSeats { get; set; }

        public List<Country>? Countries { get; set; }
    }
}
