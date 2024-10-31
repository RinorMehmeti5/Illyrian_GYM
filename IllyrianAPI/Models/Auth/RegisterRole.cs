using System.ComponentModel.DataAnnotations;

namespace IllyrianAPI.Models.Auth
{
    public class RegisterRole
    {
        [Required]
        [StringLength(128)]
        public string Name_SQ { get; set; } = null!;

        [Required]
        [StringLength(128)]
        public string Name_EN { get; set; } = null!;

        [StringLength(4000)]
        public string? Description { get; set; }
    }
}
