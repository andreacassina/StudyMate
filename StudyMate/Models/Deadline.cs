using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyMate.Models
{
    public partial class Deadline
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime ExamDate { get; set; }

        public virtual Course Course { get; set; } = null!;
    }
}
