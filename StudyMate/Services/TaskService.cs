using StudyMate.Models;

namespace StudyMate.Services
{
    public class TaskService : ITaskService
    {
        private PACContext _context;

        public TaskService(PACContext pACContext)
        {
            _context = pACContext;
        }

        public Event AddTask(Event task)
        {
            _context.Add(task);
            _context.SaveChanges();
            return task;
        }

        // Cerco l'elenco delle lezioni
        public IEnumerable<EventViewModel> GetAllLessons()
        {
            List<EventViewModel> result = new List<EventViewModel>();
            var degreeCourse = "Informatica";
            
            var courseList = _context.Courses.Where(c => c.DegreeCourse.Equals(degreeCourse)).Select(c => c.CourseId);
            List<EventViewModel> lessonEvents = _context.Lessons.Where(l => courseList.Contains(l.CourseId)).Select(l => new EventViewModel
            {
                Description = l.Description,
                StartDate = l.Start,
                EndDate = l.End,
                Priority = 5
            }).ToList<EventViewModel>();

            result.AddRange(lessonEvents);
            return result;
        }

        //Cerco l'elenco degli eventi personali
        public IEnumerable<EventViewModel> GetAllPersonalEvents()
        {
            List<EventViewModel> result = new List<EventViewModel>();
            var userId = "aa11";

            List<EventViewModel> personalEvents = _context.Events.Where(e => e.UserId.Equals(userId)).Select(e => new EventViewModel
            {
                Id = e.Id,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Priority = e.Priority
            }).ToList();
            result.AddRange(personalEvents);
            return result;
        }

        // Ottengo tutti gli impegni dell'utente loggato -> lezioni + impegni personali
        public IEnumerable<EventViewModel> GetAllTasks()
        {
            List<EventViewModel> events = new List<EventViewModel>();

            List<EventViewModel> lessonEvents = (List<EventViewModel>)GetAllLessons();
            List<EventViewModel> personalEvents = (List<EventViewModel>)GetAllPersonalEvents();

            // Aggiungo gli eventi lezione
            events.AddRange(lessonEvents);
            // Aggiungo gli eventi personali
            events.AddRange(personalEvents);
            return events;
        }

        public Event GetTaskById(int id)
        {
            Event ev = _context.Events.Where(e => e.Id.Equals(id)).FirstOrDefault();
            return ev;
        }

        public int[] GetFreeHours(DateTime date)
        {
            int freeHours = 24;
                                 //00 01  02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23
            int[] day = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1 };
            //Cerco le lezioni nella giornata selezionata
            var lessons = _context.Lessons.Where(l => l.Start.Date.Equals(date.Date)).Select(l => new {
                start = l.Start.Hour,
                end = l.End.Hour
            });

            foreach(var lesson in lessons)
            {
                for (int i = lesson.start; i <= lesson.end; i++)
                    day[i] = 0;
            }

            //freeHours = day.Sum();
            //return freeHours;
            return day;
        }
    }
}
