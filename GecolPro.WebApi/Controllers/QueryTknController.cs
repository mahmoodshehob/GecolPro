using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Services.IServices;


//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using static GecolPro.Models.Models.MultiRequestUSSD;
using GecolPro.BusinessRules.Interfaces;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]

    public class QueryTknController : ControllerBase
    {

        private ILoggers _loggerG;
        private IUssdProcess _ussdProcess;




        private MultiRequest multiRequestRE = new MultiRequest();
        private const string contentType = "text/xml";


        public QueryTknController(
            ILoggers loggerG,
            IUssdProcess ussdProcess)
        {
             _loggerG = loggerG;
            _ussdProcess = ussdProcess;
        }



        private async Task<string> RemoteIpAddress()
        {
            string? remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (remoteIpAddress == "127.0.0.1" || remoteIpAddress == "::1")
            {
                // Get the original IP from the X-Forwarded-For header
                remoteIpAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            remoteIpAddress = remoteIpAddress == null ? "127.0.0.1" : "127.11.11.1";

            return remoteIpAddress;
        }


        #region API Region 

        //English

        [HttpPost]
        [Consumes(contentType)]
        [Route("api/[Controller]/V1/En")]
        public async Task<ContentResult> PostV1En()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                response = await _ussdProcess.GetQueryTokensResponse(xmlContent, "En");

                return response;
            }

        }

        //Arabic

        [HttpPost]
        [Consumes(contentType)]
        [Route("api/[Controller]/V1/Ar")]
        public async Task<ContentResult> PostV1Ar()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                response = await _ussdProcess.GetQueryTokensResponse(xmlContent, "Ar");

                return response;
            }
        }

        #endregion

    }
}

