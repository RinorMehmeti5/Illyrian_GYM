using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Menu
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string NameSq { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string NameSr { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public int? Type { get; set; }

    public int? OrderNo { get; set; }

    public bool Active { get; set; }

    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual AspNetUsers CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

    public virtual AspNetUsers? ModifiedByNavigation { get; set; }

    public virtual Menu? Parent { get; set; }

    public virtual ICollection<RoleMenus> RoleMenus { get; set; } = new List<RoleMenus>();
}
