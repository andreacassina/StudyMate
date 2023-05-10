using Microsoft.AspNetCore.Mvc;
using StudyMate.Models;
using StudyMate.Services;
using System.Diagnostics;

namespace StudyMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;

        public HomeController(ILogger<HomeController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            DateTime date = new DateTime(2022, 10, 05);
            int a = _taskService.GetFreeHours(date);
            Console.WriteLine(a);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}