//using GecolPro.Main.Models;
using ClassLibrary.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GecolPro.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {



            return View();
        }

        public async Task<IActionResult> Privacy()
        {




            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult systemOp()
        {

            string OS = Environment.OSVersion.ToString();

            string username = Environment.UserName;
            string Message = $"The OS Version : {OS}\nThe User is : {username}";

            return Content(Message);
        }




    }
}