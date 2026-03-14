namespace Illyrian.Domain.Entities;

public class User
{
    public string Id { get; set; } = null!;
    public string? PersonalNumber { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Birthdate { get; set; }
    public DateTime? PasswordExpires { get; set; }
    public bool? AllowNotifications { get; set; }
    public bool? AllowAdminNotification { get; set; }
    public int? CityId { get; set; }
    public int? SettlementId { get; set; }
    public string? Address { get; set; }
    public string? GoogleToken { get; set; }
    public bool? Active { get; set; }
    public string? InsertedFrom { get; set; }
    public DateTime InsertedDate { get; set; }
    public string? ImageProfile { get; set; }
    public int? LanguageId { get; set; }

    public Language? Language { get; set; }
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<UserClass> UserClasses { get; set; } = new List<UserClass>();
    public ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
    public ICollection<LogEntry> Logs { get; set; } = new List<LogEntry>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
