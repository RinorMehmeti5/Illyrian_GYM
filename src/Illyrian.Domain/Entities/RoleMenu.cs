namespace Illyrian.Domain.Entities;

public class RoleMenu
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public string RoleId { get; set; } = null!;
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = null!;

    public Menu Menu { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public User CreatedByNavigation { get; set; } = null!;
    public User? ModifiedByNavigation { get; set; }
}
