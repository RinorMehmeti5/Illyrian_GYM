namespace Illyrian.Persistence.Auth;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string? UserRole { get; set; }
    public string? UserLanguage { get; set; }
    public DateTime Expiration { get; set; }
}
