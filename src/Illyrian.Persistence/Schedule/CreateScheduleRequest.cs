namespace Illyrian.Persistence.Schedule;

public class CreateScheduleRequest
{
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public string DayOfWeek { get; set; } = null!;
}
