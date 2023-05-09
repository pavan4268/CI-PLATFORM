using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class PrivacyPolicyVm
    {
        public long CmsPageId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string Slug { get; set; } = null!;
    }
}
