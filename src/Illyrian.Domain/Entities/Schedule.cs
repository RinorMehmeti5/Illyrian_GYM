namespace Illyrian.Domain.Entities;

public class Schedule
{
    public int ScheduleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DayOfWeek { get; set; } = null!;

    public ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
}
