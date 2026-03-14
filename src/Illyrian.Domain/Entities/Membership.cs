namespace Illyrian.Domain.Entities;

public class Membership
{
    public int MembershipId { get; set; }
    public string? UserId { get; set; }
    public int MembershipTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? IsActive { get; set; }

    public User? User { get; set; }
    public MembershipType? MembershipType { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
