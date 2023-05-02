namespace StudyMate.Services
{
    public interface IDegreeCourseService
    {
        bool searchDegreeCourseCourses(string degreeCourse);       //Funzione che cerca se esistono i corsi di uno specifico corso di laurea (do per scontato che se ne esiste uno esistono tutti)
        void DownloadCourses(string degreeCourse);
    }
}
