namespace Illyrian.Domain.Entities;

public class UserSchedule
{
    public int UserScheduleId { get; set; }
    public string UserId { get; set; } = null!;
    public int ScheduleId { get; set; }

    public User User { get; set; } = null!;
    public Schedule Schedule { get; set; } = null!;
}
