using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionThemeCreateVm
    {
        public long MissionThemeId { get; set; }


        [Required(ErrorMessage = "Theme Name is Required")]
        [RegularExpression("^[a-zA-Z& ]+$", ErrorMessage = "Please enter only alphabetic characters, spaces, and '&' symbol")]
        public string? Title { get; set; } = null!;

        public byte Status { get; set; }
    }
}
