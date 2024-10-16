//using GecolPro.Main.Models;
using ClassLibrary.Models.Models;
using ConsoleApp_GecolSystem;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using ClassLibrary.GecolSystem;

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

        public IActionResult Test()
        {
            string Message = "{}";

            //string xmlFile = new ReadXmlFile().Read();

            //string ddd =  new DeSerlV1().DoDeSerl(xmlFile);

            //var _xmlServices = new XmlServices();

            //var resp = _xmlServices.ToCreditVendCRsp(xmlFile);

            //string respToJson = System.Text.Json.JsonSerializer.Serialize(resp);

            //Message = respToJson;

            return Content(Message, "application/json");

        }





    }
}