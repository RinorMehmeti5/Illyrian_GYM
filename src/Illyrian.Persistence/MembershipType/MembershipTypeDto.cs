namespace Illyrian.Persistence.MembershipType;

public class MembershipTypeDto
{
    public int MembershipTypeID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int DurationInDays { get; set; }
    public decimal Price { get; set; }
    public string? FormattedDuration { get; set; }
    public string? FormattedPrice { get; set; }
}
