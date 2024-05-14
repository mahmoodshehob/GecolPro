using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClassLibrary.Services;
using System.Net.Mail;
using GecolPro.Main.Models;

namespace GecolPro.Main.Controllers
{
    [Route("api/[controller]/[action]/v1")] // Corrected route
    [ApiController]
    public class KannelGwController : ControllerBase
    {

        [HttpPost]
        //api/KannelGw/PostMessage/v1
        public async Task<IActionResult> PostMessage([FromBody] SmsMessage messageData)
        {
            ServiceProcess.UssdProcessV1.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message);

            return StatusCode(200);
            //if (result.StatusCode == "200")
            //{
            //    return StatusCode(200, result.Responce);
            //}
            //else
            //{
            //    return StatusCode(500, result.Responce);
            //}
        }
    }
}