using Microsoft.AspNetCore.Mvc;
using GecolPro.Models.Models;
using GecolPro.WebApi.BusinessRules;
using GecolPro.WebApi.Interfaces;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]/v1")] // Corrected route
    public class KannelGwController : ControllerBase
    {

        private ISendMessage _sendMessage;

        public KannelGwController(ISendMessage sendMessage)
        {
            _sendMessage = sendMessage;
        }

        [HttpPost]
        //api/KannelGw/PostMessage/v1
        public async Task<IActionResult> PostMessage([FromBody] SmsMessage messageData)
        {
            //BusinessRules.UssdProcessV1.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message);

            _sendMessage.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message,"0000000099");

            return StatusCode(200);
        }
    }
}