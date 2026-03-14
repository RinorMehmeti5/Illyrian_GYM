namespace Illyrian.Domain.Entities;

public class GymClass
{
    public int ClassId { get; set; }
    public string? ClassName { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
    public TimeOnly? ScheduleTime { get; set; }
    public string? ScheduleDay { get; set; }

    public ICollection<UserClass> UserClasses { get; set; } = new List<UserClass>();
}
