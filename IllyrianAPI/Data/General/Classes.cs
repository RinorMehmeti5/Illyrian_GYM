using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Classes
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string? Description { get; set; }

    public int? Capacity { get; set; }

    public TimeOnly? ScheduleTime { get; set; }

    public string? ScheduleDay { get; set; }

    public virtual ICollection<UserClasses> UserClasses { get; set; } = new List<UserClasses>();
}
