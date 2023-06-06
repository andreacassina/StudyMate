using Microsoft.AspNetCore.Identity;
using StudyMate.Models;
using System.Text.Json;


namespace StudyMate.Services
{
    public class TaskService : ITaskService
    {
        private PACContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskService(PACContext pACContext, UserManager<ApplicationUser> userManager)
        {
            _context = pACContext;
            _userManager = userManager;
        }

        public Event AddTask(Event task)
        {
            _context.Add(task);
            _context.SaveChanges();
            return task;
        }

        // Cerco l'elenco delle lezioni
        public IEnumerable<EventViewModel> GetAllLessons(string degreeCourse)
        {
            List<EventViewModel> result = new List<EventViewModel>();
            
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
        public IEnumerable<EventViewModel> GetAllPersonalEvents(string userId)
        {
            List<EventViewModel> result = new List<EventViewModel>();

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

        public Event GetTaskById(int id)
        {
            Event ev = _context.Events.Where(e => e.Id.Equals(id)).FirstOrDefault();
            return ev;
        }

        public int[] GetFreeHours(DateTime date)
        {
            int freeHours = 24;
                                 //00 01  02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23
            int[] day = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0 };
            //Cerco le lezioni nella giornata selezionata
            var lessons = _context.Lessons.Where(l => l.Start.Date.Equals(date.Date)).Select(l => new {
                start = l.Start.Hour,
                end = l.End.Hour
            });

            var personalEvents = _context.Events.Where(e => e.StartDate.Date.Equals(date.Date)).Select(e => new {
                start = e.StartDate.Hour,
                end = e.EndDate.Hour
            });

            

            foreach (var lesson in lessons)
            {
                for (int i = lesson.start; i <= lesson.end; i++)
                    day[i] = 0;
            }

            foreach(var per in personalEvents)
            {
                for (int i = per.start; i <= per.end; i++)
                    day[i] = 0;
            }

            //freeHours = day.Sum();
            //return freeHours;
            return day;
        }

        // Elenco di tutti gli impegni + studyslots assegnati + deadlines
        List<CalendarEventDTO> ITaskService.GetCalendarEvents(string userId)
        {
            List<CalendarEventDTO> calendarEvents = new List<CalendarEventDTO>();
            //Ottengo il corso di laurea dell'utente loggato
            string degreeCourse = _userManager.Users.Where(u => u.Id.Equals(userId)).Select(u => u.DegreeCourse).First();

            // Ottengo l'elenco dei corsi del degreeCouse
            var courses = _context.Courses.Where(c => c.DegreeCourse.Equals(degreeCourse)).Select(c => new{ c.CourseId, c.CourseName});

            foreach(var course in courses)
            {
                var lessons = _context.Lessons.Where(l => l.CourseId.Equals(course.CourseId)).Select(l => new CalendarEventDTO
                {
                    title = l.Description,
                    start = l.Start,
                    end = l.End,
                    backgroundColor = "blue",
                    borderColor = "darkblue",
                    textColor = "white"

                }).ToList();

                //Inserisco le date degli esami
                var deadlines = _context.Deadlines.Where(d => d.CourseId.Equals(course.CourseId)).Select(d => new CalendarEventDTO
                {
                    title = course.CourseName,
                    start = d.ExamDate,
                    end = d.ExamDate.AddHours(4),
                    backgroundColor = "red",
                    borderColor = "black",
                    textColor = "white"
                }).ToList();



                calendarEvents.AddRange(lessons);
                calendarEvents.AddRange(deadlines);
            }

            var studySlots = _context.StudySlots.Where(s => s.UserId.Equals(userId)).Select(s => new CalendarEventDTO
            {
                title = s.CourseName,
                start = s.From,
                end = s.To,
                backgroundColor = "green",
                borderColor = "darkgreen",
                textColor = "white"
            }).ToList();

            calendarEvents.AddRange(studySlots);

            //inserisco gli eventi personali
            var personalEvents = _context.Events.Where(e => e.UserId.Equals(userId)).Select(e => new CalendarEventDTO
            {
                title = e.Description,
                start = e.StartDate,
                end = e.EndDate,
                backgroundColor = "lightblue",
                borderColor = "darkblue",
                textColor = "white"
            }).ToList();

            calendarEvents.AddRange(personalEvents);






            //var calendarEventsJSON = JsonSerializer.Serialize(calendarEvents);
            //return calendarEventsJSON;
            return calendarEvents;

            
        }

    }
}
