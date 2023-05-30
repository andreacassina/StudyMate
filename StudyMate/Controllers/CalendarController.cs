using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Controllers
{
    [Route("api/[controller]")]
    public class CalendarController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarController(ITaskService taskService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("events")]
        public IActionResult GetEvents()
        {
            var eventsJSON = _taskService.GetCalendarEvents(_userManager.GetUserId(User));
            return Json(eventsJSON);
        }
    }
}
