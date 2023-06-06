using Microsoft.AspNetCore.Mvc;
using StudyMate.Models;
using StudyMate.Services;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace StudyMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;
        private readonly IGreedyService _greedyService;
        private readonly UserManager<ApplicationUser> _userManager;    

        public HomeController(ILogger<HomeController> logger, ITaskService taskService, IGreedyService greedyService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _taskService = taskService;
            _greedyService = greedyService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> CalculateStudyTime()
        public async Task<IActionResult> CalculateStudyTime()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);


            //DateTime date = new DateTime(2022, 10, 05);
            //DateTime date = new DateTime(2022, 10, 06);
            DateTime date = new DateTime(2023, 06, 05);

            if (user != null)
            {
                string userName = user.UserName;
                string userId = user.Id;
                string degreeCourse = user.DegreeCourse;
                int[] weekHours = new int[168]; // totale ore in 7 giorni

                // Elimino gli study slots già inseriti per l'utente
                await _greedyService.deleteStudySlot(date, user.Id);

                int index = 0;
                for(int i = 0; i < 7; i++)
                {
                    DateTime dt = date.AddDays(i);
                    int[] dailyHours = _taskService.GetFreeHours(dt);

                    for (int j = 0; j < dailyHours.Length; j++)
                    {
                        weekHours[index] = dailyHours[j];
                        index++;
                    }

                }
                //_taskService.GetFreeHours(date);
                


                if (_greedyService.CalculateGreedy(weekHours, user.Id, user.UserName, user.DegreeCourse, date))
                    Console.WriteLine("OK");
                else
                    Console.WriteLine("ERRORE");

            }
            //var claims = User.Claims.Where(c => c.Type.Equals("UserName")).FirstOrDefault();
            

            //if(_greedyService.CalculateGreedy(dailyHours, User.Identity.Name,))

            
            return View("Index");
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