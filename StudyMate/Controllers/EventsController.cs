using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            List<EventViewModel> events = (List<EventViewModel>)_taskService.GetAllTasks();
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
            List<SelectListItem> courses = new List<SelectListItem>()
            {
                // Creo dei corsi in modo temporaneo
                new SelectListItem()
                {
                    Text = "PAC",
                    Value = "1"
                }
            };

            //ViewBag.userId = new SelectListItem()
            //{
            //    Text = "PippoPuppo",
            //    Value = "1"
            //};
           
            ViewBag.CourseId = courses;
            return View();
        }

        // POST: EventsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        public ActionResult Create(Event ev)
        {
            try
            {
                ev.UserId = "aa11";
                _taskService.AddTask(ev);
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

        //Elenco di tutti gli eventiPersonali
        public ActionResult PersonalEvents()
        {
            List<EventViewModel> events = (List<EventViewModel>)_taskService.GetAllPersonalEvents();
            return View(events);
        }

        //Elenco di tutte le lezioni del percorso di studi
        public ActionResult LessonEvents()
        {
            List<EventViewModel> events = (List<EventViewModel>)_taskService.GetAllLessons();
            return View("Lessons",events);
        }

    }
}
