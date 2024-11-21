using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class RoleMenus
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public string RoleId { get; set; } = null!;

    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual AspNetUsers CreatedByNavigation { get; set; } = null!;

    public virtual Menu Menu { get; set; } = null!;

    public virtual AspNetUsers? ModifiedByNavigation { get; set; }

    public virtual AspNetRoles Role { get; set; } = null!;
}
