using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class MembershipTypes
{
    public int MembershipTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Memberships> Memberships { get; set; } = new List<Memberships>();
}
