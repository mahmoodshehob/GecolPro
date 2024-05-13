using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClassLibrary.Services;



namespace GecolPro.Main.Controllers
{
    [Route("api/[controller]/[action]/v1")] // Corrected route
    [ApiController]
    public class KannelGwController : ControllerBase
    {

        [HttpPost]
        //api/KannelGw/PostMessage/v1
        public async Task<IActionResult> PostMessage([FromBody] dynamic messageData)
        {
            ServiceProcess.UssdProcessV1.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message);

            var result = await ServiceProcess.UssdProcessV1.SendGecolMessage(null, messageData.Receiver, messageData.Message);

            if (result.StatusCode == "200")
            {
                return StatusCode(200, result.Responce);
            }
            else
            {
                return StatusCode(500, result.Responce);
            }
        }
    }
}