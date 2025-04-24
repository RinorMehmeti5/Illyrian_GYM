namespace IllyrianAPI.Models.Schedule
{
    public class UpdateScheduleRequest
    {
        public int ScheduleId { get; set; }
        public string StartTime { get; set; } // Format: "HH:MM"
        public string EndTime { get; set; } // Format: "HH:MM"
        public string DayOfWeek { get; set; }
    }
}