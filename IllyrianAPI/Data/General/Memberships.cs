using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Memberships
{
    public int MembershipId { get; set; }

    public string UserId { get; set; } = null!;

    public int MembershipTypeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual MembershipTypes MembershipType { get; set; } = null!;

    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();

    public virtual AspNetUsers User { get; set; } = null!;
}
