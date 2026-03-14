using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Illyrian.Persistence.General;

public class ApplicationRole : IdentityRole
{
    [Required, StringLength(128)]
    public string Name_SQ { get; set; } = null!;

    [Required, StringLength(128)]
    public string Name_EN { get; set; } = null!;

    [StringLength(4000)]
    public string? Description { get; set; }
}
