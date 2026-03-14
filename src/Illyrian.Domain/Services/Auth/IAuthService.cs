using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Services.Auth;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(RegisterModel model);
    Task<AuthResult> RegisterRoleAsync(string nameSq, string nameEn, string? description);
    Task LogoutAsync();
    Task<AuthResult> ResetPasswordAsync(string userId, string newPassword);
    Task<AuthResult> CreateUserAsync(CreateUserModel model);
    Task<AuthResult> UpdateUserAsync(string userId, UpdateUserModel model);
    Task<AuthResult> DeleteUserAsync(string userId);
    Task<Entities.User?> FindUserByIdAsync(string id);
    Task<Entities.User?> FindUserByEmailAsync(string email);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task<IEnumerable<Entities.Role>> GetRolesAsync();
    Task<IEnumerable<Entities.User>> GetAllUsersWithRolesAsync();
}

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? UserId { get; set; }
    public string? UserRole { get; set; }
    public string? UserLanguage { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class RegisterModel
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Role { get; set; }
    public string? PersonalNumber { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateTime? Birthdate { get; set; }
    public int? CityId { get; set; }
    public int? SettlementId { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}

public class CreateUserModel
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

public class UpdateUserModel
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
