using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Data;

public partial class Notification
{
    public long NotifId { get; set; }

    public long FromUserId { get; set; }

    public long ToUserId { get; set; }

    public long? NotifTypeId { get; set; }

    public string Message { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User FromUser { get; set; } = null!;

    public virtual NotificationType? NotifType { get; set; }

    public virtual User ToUser { get; set; } = null!;
}
