namespace Illyrian.Domain.Entities;

public class StatusType
{
    public int StatusTypeId { get; set; }
    public string NameSq { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string NameSr { get; set; } = null!;
    public bool? Active { get; set; }
    public string InsertedFrom { get; set; } = null!;
    public DateTime InsertedDate { get; set; }
    public string? UpdatedFrom { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public User InsertedFromNavigation { get; set; } = null!;
    public User? UpdatedFromNavigation { get; set; }
}
