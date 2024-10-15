using GecolPro.WebApi.BusinessRules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace GecolPro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackListController : Controller
    {
        private BlackListFun blackListFun = new BlackListFun();

        public BlackListController()
        {

        }



        [HttpPut()]
        [Consumes("application/json")]
        public async Task<IActionResult> DbToBlackList()
        {
            return  Ok("no db now");
        }



        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddToBlackList([FromBody] string blackMsisdn)
        {
            if (blackMsisdn == null)
            {
                return BadRequest("Invalid phone numbers.");
            }

            List<string> blackMsisdnList = new List<string>();

            blackMsisdnList.Add(blackMsisdn);

            await blackListFun.SyncBlackList(blackMsisdnList);


            return Ok(blackMsisdn);
        }



        [HttpPost("addMultiple")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddMultipleToBlackList([FromBody] List<string> blackMsisdnList)
        {
            if (blackMsisdnList == null || blackMsisdnList.Count == 0)
            {
                return BadRequest("Invalid phone numbers list.");
            }

            await blackListFun.SyncBlackList(blackMsisdnList);

            return Ok(blackMsisdnList);
        }



        [HttpGet("list")]
        public async Task<IActionResult> GetBlackList()
        {

            return Ok(await blackListFun.GetBlackList());
        }



        [HttpDelete("{phoneNumber}")]
        public async Task<ActionResult> RemoveFromBlacklist(string phoneNumber)
        {
            await blackListFun.SyncDeleteBlackList(phoneNumber);

            return NoContent();
        }
        
    }
}