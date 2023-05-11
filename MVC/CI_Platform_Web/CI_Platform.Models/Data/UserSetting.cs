using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.Data;

public partial class UserSetting
{
    public long UserSettingsId { get; set; }

    public long UserId { get; set; }

    public long NotifSettingId { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? DeletedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public virtual NotificationType NotifSetting { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
