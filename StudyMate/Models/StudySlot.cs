using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMate.Models
{
    public class StudySlot
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime From { get; set; }      // Orario di inizio
        public DateTime To { get; set; }        // Orario di fine
        public int Hours { get; set; }
        [NotMapped]
        public double Priority { get; set; }
    }   
}
