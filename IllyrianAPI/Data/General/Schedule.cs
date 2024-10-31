using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public virtual ICollection<UsersSchedule> UsersSchedule { get; set; } = new List<UsersSchedule>();
}
