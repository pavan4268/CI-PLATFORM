using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class NotificationVm
    {
        public long NotifId { get; set; }

        public long? NotifTypeId { get; set; }

        public string Message { get; set; } = null!;

        public string Url { get; set; } = null!;

        public int Status { get; set; }

        public string? FromUserAvtaar { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
