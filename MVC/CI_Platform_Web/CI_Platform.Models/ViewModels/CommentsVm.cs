using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class CommentsVm
    {
        public string CommentText { get; set; }

        public long CommentId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }
    }
}
