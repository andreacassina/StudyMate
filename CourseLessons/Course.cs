namespace CourseLessons
{
    public class Course
    {
        public string CourseName { get; set; } = null!;
        public string ProfessorName { get; set; } = null!;
        public string DegreeCourse { get; set; } = null!;
        public int Cfu { get; set; }
        public DateTime ExamDate1 { get; set; }
       
        //public DateTime FirstLessonDate { get; set; }
        //public DateTime LastLessonDate { get; set; }
        //public List<Lesson> Lessons { get; set; }   // Elenco delle lezioni
    }
}
