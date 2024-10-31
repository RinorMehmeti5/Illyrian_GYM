using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class UserClasses
{
    public int UserClassId { get; set; }

    public string UserId { get; set; } = null!;

    public int ClassId { get; set; }

    public DateOnly? AttendanceDate { get; set; }

    public virtual Classes Class { get; set; } = null!;

    public virtual AspNetUsers User { get; set; } = null!;
}
