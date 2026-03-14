namespace Illyrian.Domain.Configuration;

public class JwtSettings
{
    public string Secret { get; set; } = null!;
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public int ExpirationInHours { get; set; } = 3;
}
