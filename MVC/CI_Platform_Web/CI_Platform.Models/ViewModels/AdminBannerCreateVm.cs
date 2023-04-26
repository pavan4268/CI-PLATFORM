using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminBannerCreateVm
    {
        public long? BannerId { get; set; }

        public string? Image { get; set; }

        public string? Text { get; set; }

        public int? SortOrder { get; set; }

        public IFormFile? BannerImage { get; set; }
    }
}
