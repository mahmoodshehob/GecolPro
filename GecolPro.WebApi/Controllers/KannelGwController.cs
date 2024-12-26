using Microsoft.AspNetCore.Mvc;
using GecolPro.Models.Models;
using GecolPro.BusinessRules.Interfaces;

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
            await _sendMessage.SendGecolMessage(messageData.Receiver, messageData.Message,"0000000099");

            return StatusCode(200);
        }

        [HttpPost]
        //api/KannelGw/PostMessage/v1
        public async Task<IActionResult> PostMessageTest([FromBody] SmsMessage messageData)
        {
            await _sendMessage.SendGecolMessageTest(messageData.Receiver, messageData.Message, "0000000099");

            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessageWR([FromBody] SmsMessage messageData)
        {
            var _ = await _sendMessage.SendGecolMessageWR(messageData.Receiver, messageData.Message, "0000000099");
            if (_.Item1)
            {
                return StatusCode(200);

            }


            return StatusCode(400);
        }
    }
}