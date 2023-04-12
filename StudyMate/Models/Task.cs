namespace StudyMate.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string taskName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime taskFrom { get; set; }
        public DateTime taskTo { get; set; }
        public int priority { get; set; }
        public int cadence { get; set; }
    }
}
