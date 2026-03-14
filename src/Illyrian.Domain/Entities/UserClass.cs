namespace Illyrian.Domain.Entities;

public class UserClass
{
    public int UserClassId { get; set; }
    public string UserId { get; set; } = null!;
    public int ClassId { get; set; }
    public DateOnly? AttendanceDate { get; set; }

    public User User { get; set; } = null!;
    public GymClass Class { get; set; } = null!;
}
