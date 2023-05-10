using System.ComponentModel.DataAnnotations;

namespace StudyMate.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; } 

        public virtual Course Course { get; set; } = null!;
    }
}
