using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class AspNetUsers
{
    public string Id { get; set; } = null!;

    public string? PersonalNumber { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public DateTime? Birthdate { get; set; }

    public DateTime PasswordExpires { get; set; }

    public bool AllowNotifications { get; set; }

    public bool AllowAdminNotification { get; set; }

    public int? CityId { get; set; }

    public int? SettlementId { get; set; }

    public string? Address { get; set; }

    public string? GoogleToken { get; set; }

    public bool? Active { get; set; }

    public string? InsertedFrom { get; set; }

    public DateTime InsertedDate { get; set; }

    public string? ImageProfile { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int? AccessFailedCount { get; set; }

    public int? LanguageId { get; set; }

    public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; } = new List<AspNetUserClaims>();

    public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; } = new List<AspNetUserLogins>();

    public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; } = new List<AspNetUserTokens>();

    public virtual Language? Language { get; set; }

    public virtual ICollection<Memberships> Memberships { get; set; } = new List<Memberships>();

    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();

    public virtual ICollection<UserClasses> UserClasses { get; set; } = new List<UserClasses>();

    public virtual ICollection<UsersSchedule> UsersSchedule { get; set; } = new List<UsersSchedule>();

    public virtual ICollection<AspNetRoles> Role { get; set; } = new List<AspNetRoles>();
}
