using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Controllers
{
    public class EventsController : Controller
    {
        private readonly ITaskService _taskService;
        private PACContext _context;

        public EventsController(ITaskService taskService, PACContext pACContext)
        {
            _taskService = taskService;
            _context = pACContext;
        }
        // GET: EventsController
        public ActionResult Index()
        {
            List<Event> events = (List<Event>)_taskService.GetAllTasks();
            return View(events);
        }

        // GET: EventsController/Details/5
        public ActionResult Details(int id)
        {
            Event ev = new Event();
            ev = _context.Events.Where(e => e.Id.Equals(id)).FirstOrDefault();
            return View(ev);
        }

        // GET: EventsController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: EventsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
