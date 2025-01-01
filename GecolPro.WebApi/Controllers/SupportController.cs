using GecolPro.BusinessRules.BusinessRules;
using GecolPro.BusinessRules.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GecolPro.WebApi.Controllers
{
    public class SupportController : Controller
    {

        private IUssdProcess _ussdProcess;

        public SupportController(IUssdProcess ussdProcess)
        {
            _ussdProcess = ussdProcess; 
        }


        [HttpPost]
        [Route("api/[controller]/[Action]")]
        public async Task<IActionResult> QueryTkn( string? Msisdn , string OrderdNumber , string Language ="Ar")
        {

            if (string.IsNullOrEmpty(Msisdn) || string.IsNullOrWhiteSpace(Msisdn) || long.TryParse(Msisdn, out _) )
            {
                Msisdn = "218947776156";
            }


            var _result = await _ussdProcess.GetQueryTokensResponseSupport(Msisdn, OrderdNumber, Language); 
             

            if (_result.Item1)
            {
                return Ok(_result.Item2);
            }
            else
            {
                return BadRequest(_result.Item2);
            }

        }
    }
}
