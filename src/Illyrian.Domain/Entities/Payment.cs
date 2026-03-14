namespace Illyrian.Domain.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public string UserId { get; set; } = null!;
    public int MembershipId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
    public Membership Membership { get; set; } = null!;
}
