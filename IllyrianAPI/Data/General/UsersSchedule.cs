using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class UsersSchedule
{
    public int UserScheduleId { get; set; }

    public string UserId { get; set; } = null!;

    public int ScheduleId { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual AspNetUsers User { get; set; } = null!;
}
