namespace Illyrian.Domain.Entities;

public class Menu
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string NameSq { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string NameSr { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int? Type { get; set; }
    public int? OrderNo { get; set; }
    public bool? Active { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = null!;

    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    public User CreatedByNavigation { get; set; } = null!;
    public User? ModifiedByNavigation { get; set; }
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}
