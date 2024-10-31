namespace IllyrianAPI.Models.Payment
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public string? UserId { get; set; }
        public string? UserFullName { get; set; }
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
}
