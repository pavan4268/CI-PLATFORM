using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Data;

public partial class NotificationType
{
    public long NotificationTypeId { get; set; }

    public string SettingsTitle { get; set; } = null!;

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<UserSetting> UserSettings { get; } = new List<UserSetting>();
}
