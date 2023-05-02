using Newtonsoft.Json;
using StudyMate.Models;

namespace StudyMate.Services
{
    public class DegreeCourseService : IDegreeCourseService
    {
        private PACContext _context;
        private readonly HttpClient _httpClient;

        public DegreeCourseService(PACContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public bool searchDegreeCourseCourses(string degreeCourse)
        {
            var res = _context.Courses.Where(c => c.DegreeCourse == degreeCourse).ToList();
            if(res.Count() == 0)
            {
                return false;
            }
            else
                return true;
        }

        //Ottengo l'elenco di tutti i corsi dato il corso di laurea -> l'elenco dei corsi viene preso dall'API CourseLesson
        public async void DownloadCourses(string degreeCourse)
        {
            var apiUrl = $"https://localhost:7141/GetCourses?degreeCourse=" + degreeCourse;

            var response = _httpClient.GetAsync(apiUrl).Result;

            response.EnsureSuccessStatusCode();

            //IEnumerable<Course> courses = await response.Content.ReadAsStringAsync();
            var responseString = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<Course[]>(responseString);

            //PARTE DI INERIMENTO NEL DB E RICERCA DELLE LEZIONI
        }

        // Ottengo l'elenco delle lezioni dato il nome del corso -> salvo nel db
        private void DownloadLessonsByCourse(string courseName)
        {

        }
    }
}
