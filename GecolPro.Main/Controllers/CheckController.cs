using GecolPro.Main.UssdService;
using Microsoft.AspNetCore.Mvc;
using static ClassLibrary.Models.Models.MultiRequestUSSD;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using GecolPro.Main.BusinessRules;

namespace GecolPro.Main.Controllers
{
    public class CheckController : ControllerBase
    {

  
        [HttpGet()]
        [Route("api/{Controller}/VendService")]
        public async Task<IActionResult> VendService()
        {
            bool Status = await UssdProcessV1.CheckServiceExist();

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
