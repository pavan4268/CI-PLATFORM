using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminCMSCreateVm
    {
        public long CmsPageId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage ="Slug Cannot be Empty.")]
        public string Slug { get; set; } = null!;

        public int? Status { get; set; }
    }
}
