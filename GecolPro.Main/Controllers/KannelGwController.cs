﻿using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models.Models;
using GecolPro.Main.BusinessRules;

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
            //BusinessRules.UssdProcessV1.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message);
            
            SendMessage.SendGecolMessage("2188997772", messageData.Receiver, messageData.Message,"0000000099");

            return StatusCode(200);
        }
    }
}