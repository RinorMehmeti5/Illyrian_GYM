namespace IllyrianAPI.Models.Class
{
    public class Class
    {
        public int ClassID { get; set; }
        public string? ClassName { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public string? ScheduleDay { get; set; }
    }
}
