namespace IllyrianAPI.Models.Membership
{
    public class MembershipDTO
    {
        public int MembershipId { get; set; }
        public string? UserId { get; set; }
        public string? UserFullName { get; set; }
        public int MembershipTypeId { get; set; }
        public string? MembershipTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public string? FormattedPrice { get; set; }
        public string? FormattedStartDate { get; set; }
        public string? FormattedEndDate { get; set; }
        public int DurationInDays { get; set; }
        public string? FormattedDuration { get; set; }
    }
}
