using Microsoft.AspNetCore.Mvc;
using GecolPro.WebApi.Interfaces;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]/")]
    public class CheckController : ControllerBase
    {
        private IUssdProcess _ussdProcess;


        public CheckController(IUssdProcess ussdProcess) 
        {
            _ussdProcess = ussdProcess;
        }



        #region API Region 

        [HttpGet("VendService", Name = "VendService")]

        public async Task<IActionResult> VendService()
        {


            string? remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            bool Status = await _ussdProcess.CheckServiceExist(remoteIpAddress);

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

        [HttpGet("DcbService",Name = "DcbService")]
        public async Task<IActionResult> DcbService()
        {
            bool Status = await _ussdProcess.CheckDcbExist();

            string responseTime = "| " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (Status)
            {
                return Ok(new { Status = $"Service Connected {responseTime}" });
            }
            else
            {
                return BadRequest(new { Status = $"Service Down {responseTime}" });
            }
        }
        #endregion

    }
}
