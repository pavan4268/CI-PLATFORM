using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CI_Platform.Entities.ViewModels
{
    public class AdminCommentDisplayVm
    {
        public long CommentId { get; set; }

        public long UserId { get; set; }

        public string? Username { get; set; }

        public long MissionId { get; set; }

        public string? MissionTitle { get; set; }

        public string? CommentText { get; set; }
    }
}
