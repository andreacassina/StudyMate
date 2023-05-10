using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyMate.Models
{
    public partial class Event
    {
        [Key]
        public int Id { get; set; }
        //public int CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? UserId { get; set; }
        public string Description { get; set; } = null!;
        public int Priority { get; set; }

        //public virtual Course Course { get; set; } = null!;
        //public virtual User? User { get; set; }
    }
}
