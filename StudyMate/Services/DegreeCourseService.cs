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

        public IEnumerable<string> GetDegreeCoursesName()
        {
            var apiUrl = $"https://localhost:7141/GetDegreeCourses";

            var response = _httpClient.GetAsync(apiUrl).Result;

            response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync().Result;
            var deserialized = JsonConvert.DeserializeObject<IEnumerable<string>>(jsonString);
            return deserialized;
        }

        //Ottengo l'elenco di tutti i corsi dato il corso di laurea -> l'elenco dei corsi viene preso dall'API CourseLesson
        public async void DownloadCourses(string degreeCourse)
        {
            //Controllo se il corso di laurea è già presente nel database
            if (!_context.Courses.Select(c => c.DegreeCourse).Contains(degreeCourse))
            {


                var apiUrl = $"https://localhost:7141/GetCourses?degreeCourse=" + degreeCourse;

                var response = _httpClient.GetAsync(apiUrl).Result;

                response.EnsureSuccessStatusCode();

                //IEnumerable<Course> courses = await response.Content.ReadAsStringAsync();
                var responseString = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<Course[]>(responseString);

                //PARTE DI INERIMENTO NEL DB E RICERCA DELLE LEZIONI
                foreach (var course in courses)
                {
                    // Inserisco il corso nella tabella courses
                    _context.Courses.Add(course);

                    _context.SaveChanges();
                    //Ottengo le lezioni
                    DownloadLessonsByCourse(degreeCourse, course.CourseName, course.CourseId);
                    //_context.SaveChanges();
                    //Inserisco gli esami nella tabella Deadlines
                    Deadline deadline = new Deadline
                    {
                        ExamDate = course.ExamDate1,
                        CourseId = course.CourseId,
                    };

                    _context.Deadlines.Add(deadline);
                    _context.SaveChanges();
                }
            }
            

        }

        // Ottengo l'elenco delle lezioni dato il nome del corso -> salvo nel db
        private async void DownloadLessonsByCourse(string degreeCourse, string courseName, int courseID)
        {
            var apiUrl = $"https://localhost:7141/GetLessons?degreeCourse=" + degreeCourse + "&courseName=" + courseName;
            var response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var lessons = JsonConvert.DeserializeObject<Lesson[]>(responseString);

            foreach(var lesson in lessons)
            {
                lesson.CourseId = courseID;
                lesson.Description = courseName;
                _context.Lessons.Add(lesson);

            }
            _context.SaveChanges();
        }


    }
}
