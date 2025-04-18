namespace IllyrianAPI.Models.User
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string? PersonalNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool? Active { get; set; }
        public DateTime InsertedDate { get; set; }
        public string? FormattedBirthdate { get; set; }
        public string? FormattedInsertedDate { get; set; }
        public string? FullName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}