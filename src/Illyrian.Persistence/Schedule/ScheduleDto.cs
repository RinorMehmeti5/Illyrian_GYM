namespace Illyrian.Persistence.Schedule;

public class ScheduleDto
{
    public int ScheduleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DayOfWeek { get; set; } = null!;
    public string? FormattedStartTime { get; set; }
    public string? FormattedEndTime { get; set; }
    public string? FormattedDayOfWeek { get; set; }
}
