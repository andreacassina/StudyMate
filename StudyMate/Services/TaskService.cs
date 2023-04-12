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

        public IEnumerable<Event> GetAllTasks()
        {
            List<Event> events = _context.Events.ToList();
            return events;
        }

        public Event GetTaskById(int id)
        {
            Event ev = _context.Events.Where(e => e.Id.Equals(id)).FirstOrDefault();
            return ev;
        }
    }
}
