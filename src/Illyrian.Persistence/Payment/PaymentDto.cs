namespace Illyrian.Persistence.Payment;

public class PaymentDto
{
    public int PaymentId { get; set; }
    public string UserId { get; set; } = null!;
    public int MembershipId { get; set; }
    public string? MembershipTypeName { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
    public string? FormattedAmount { get; set; }
    public string? FormattedPaymentDate { get; set; }
}
