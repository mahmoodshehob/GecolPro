using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClassLibrary.Services.SmsGetaway;
using  ClassLibrary.Services.Models;
using ClassLibrary.Services.Logger;



namespace GecolPro.Main.Controllers
{
    [Route("api/[controller]/[action]/v1")] // Corrected route
    [ApiController]
    public class KannelGwController : ControllerBase
    {
        private SmsActions smsActions = new SmsActions();

        [HttpPost]
        //api/KannelGw/PostMessage/v1
        public async Task<IActionResult> PostMessage([FromBody] MessageData messageData)
        {


           
            var result = await smsActions.SubmitSms(null, messageData.Receiver,messageData.Message);

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