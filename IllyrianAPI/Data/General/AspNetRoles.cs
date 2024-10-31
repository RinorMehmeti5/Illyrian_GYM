using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class AspNetRoles
{
    public string Id { get; set; } = null!;

    public string NameSq { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaims>();

    public virtual ICollection<AspNetUsers> User { get; set; } = new List<AspNetUsers>();
}
