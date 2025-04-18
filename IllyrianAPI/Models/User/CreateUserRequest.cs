namespace IllyrianAPI.Models.User
{
    public class CreateUserRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserName { get; set; }
        public string? PersonalNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? CityId { get; set; }
        public int? SettlementId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Active { get; set; }
        public List<string>? Roles { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PersonalNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? CityId { get; set; }
        public int? SettlementId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Active { get; set; }
        public List<string>? Roles { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; } = null!;
    }

    public class RoleDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string NameSq { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Description { get; set; }
    }
}