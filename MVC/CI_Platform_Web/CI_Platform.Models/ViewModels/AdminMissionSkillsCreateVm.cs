using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionSkillsCreateVm
    {
        public long SkillId { get; set; }

        [Required(ErrorMessage = "Please Enter a Skill")]
        //[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please enter only alphabetic characters and spaces")]
        [RegularExpression("^[a-zA-Z& ]+$", ErrorMessage = "Please enter only alphabetic characters, spaces, and '&' symbol")]
        public string? SkillName { get; set; }

        public byte Status { get; set; }
    }
}
