namespace Illyrian.Domain.Entities;

public class MembershipType
{
    public int MembershipTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationInDays { get; set; }
    public decimal Price { get; set; }

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}
