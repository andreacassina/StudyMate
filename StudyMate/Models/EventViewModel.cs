namespace StudyMate.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public string? UserId { get; set; }
        public string Description { get; set; } = null!;
        public int Priority { get; set; }
    }
}
