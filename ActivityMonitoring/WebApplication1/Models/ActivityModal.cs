namespace ActivityMonitor.API.Models
{
    public class ActivityModal
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string UserName { get; set; }
        public string AppName { get; set; }
        public string ProcessName { get; set; }
        public string StartTime { get; set; }
    }

}
