using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IllyrianAPI.Data.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string? PersonalNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime PasswordExpires { get; set; }
        public bool AllowNotifications { get; set; }
        public bool AllowAdminNotification { get; set; }
        public int? CityID { get; set; }
        public int? SettlementID { get; set; }
        public string? Address { get; set; }
        [StringLength(4096)]
        public string? GoogleToken { get; set; }
        public bool? Active { get; set; }
        public string? InsertedFrom { get; set; }
        public DateTime InsertedDate { get; set; }
        public string? ImageProfile { get; set; }
        public int? LanguageID { get; set; }
    }

}
