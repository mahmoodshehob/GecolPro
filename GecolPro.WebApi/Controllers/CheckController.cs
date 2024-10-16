using Microsoft.AspNetCore.Mvc;
using GecolPro.WebApi.Interfaces;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]/VendService")]
    public class CheckController : ControllerBase
    {
        private IUssdProcessV1 _ussdProcess;


        public CheckController(IUssdProcessV1 ussdProcess) 
        {
            _ussdProcess = ussdProcess;
        }

        
        [HttpGet(Name = "VendService")]
        public async Task<IActionResult> VendService()
        {
            bool Status = await _ussdProcess.CheckServiceExist();

            string responseTime = "| " +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (Status)
            {
                return Ok(new { Status = $"Service Connected {responseTime}" });
            }
            else
            {
                return BadRequest(new { Status = $"Service Down {responseTime}" });
            }
        }

    }
}
