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
    }
}
