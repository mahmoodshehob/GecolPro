using ClassLibrary.GecolSystem;
using GecolPro.Main.Models;
using GecolPro.Main.ServiceProcess;
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
            string text = RuntimeInformation.IsOSPlatform(OSPlatform.Windows).ToString();
            
            return Content(text);
        }




    }
}