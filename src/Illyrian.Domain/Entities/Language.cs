namespace Illyrian.Domain.Entities;

public class Language
{
    public int LanguageId { get; set; }
    public string? NameSq { get; set; }
    public string? NameEn { get; set; }
    public string? Notes { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
