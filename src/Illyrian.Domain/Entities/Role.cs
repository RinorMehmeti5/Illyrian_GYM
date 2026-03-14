namespace Illyrian.Domain.Entities;

public class Role
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
    public string? NameSq { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }

    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
