namespace Illyrian.Persistence.Schedule;

public class UpdateScheduleRequest
{
    public int ScheduleId { get; set; }
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public string DayOfWeek { get; set; } = null!;
}
