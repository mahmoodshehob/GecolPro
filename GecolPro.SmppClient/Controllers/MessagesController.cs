using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using GecolPro.SmppClient.Models;
using GecolPro.SmppClient.Services;
using GecolPro.SmppClient.Services.IServices;
using System.Text.RegularExpressions;
using System.Numerics;

namespace GecolPro.SmppClient.Controllers
{


    //[Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        //private readonly ConnectionFactory _factory;


        private IServiceLogic _serviceLogic;

        public MessagesController(IServiceLogic serviceLogic)
        {
            _serviceLogic =  serviceLogic;
        }

        [HttpPost]
        [Route("api/[controller]/Post")]
        public async Task<IActionResult> Post([FromBody] SmsToKannel message)
        {

            var _result = await _serviceLogic.Post(message);
            
            if (_result.Status)
            { 
                return Ok(_result.Resulte); 
            }
            else
            {
                return BadRequest(_result.Resulte);
            }

        }


        [HttpGet("[controller]/DLR/{phone}/{msgid}/{status}/{deliveryDate}")]
        public async Task<IActionResult> DLR(string phone, string msgid, string status, string deliveryDate)
        {
            var _result = await _serviceLogic.DLR(phone , msgid , status, deliveryDate);

            if (_result.Status)
            {
                return Ok(_result.Resulte);
            }
            else
            {
                return BadRequest(_result.Resulte);
            }
        }


        [HttpGet()]
        [Route("[controller]/Status")]
        public async Task<IActionResult> KannelStatus()
        {
            var _result = await _serviceLogic.KannelStatus();

            if (_result.Status)
            {
                return Ok(_result.Resulte);
            }
            else
            {
                return BadRequest(_result.Resulte);
            }
        }

    }
}