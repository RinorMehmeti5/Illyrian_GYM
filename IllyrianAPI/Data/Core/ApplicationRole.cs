using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IllyrianAPI.Data.Core
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        [StringLength(128)]
        public string? Name_SQ { get; set; }

        [Required]
        [StringLength(128)]
        public string? Name_EN { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }
    }
}
