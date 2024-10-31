namespace IllyrianAPI.Models.Auth
{
    public class Register
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
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
}
