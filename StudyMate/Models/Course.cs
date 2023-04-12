using System;
using System.Collections.Generic;

namespace StudyMate.Models
{
    public partial class Course
    {
        public Course()
        {
            Deadlines = new HashSet<Deadline>();
            Events = new HashSet<Event>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public string ProfessorName { get; set; } = null!;
        public string DegreeCourse { get; set; } = null!;
        public int Cfu { get; set; }
        public DateTime FirstLessonDate { get; set; }
        public DateTime LastLessonDate { get; set; }

        public virtual ICollection<Deadline> Deadlines { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
