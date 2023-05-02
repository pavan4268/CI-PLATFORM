using CI_Platform.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class ForgotVm
    {
        [Required(ErrorMessage ="Please Enter an Email")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }

        public List<Banner>? banners { get; set; }
    }
}
