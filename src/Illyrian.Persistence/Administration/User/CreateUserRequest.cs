namespace Illyrian.Persistence.Administration.User;

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
