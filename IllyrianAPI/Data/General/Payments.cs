using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Payments
{
    public int PaymentId { get; set; }

    public string UserId { get; set; } = null!;

    public int MembershipId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionId { get; set; }

    public string? Notes { get; set; }

    public virtual Memberships Membership { get; set; } = null!;

    public virtual AspNetUsers User { get; set; } = null!;
}
