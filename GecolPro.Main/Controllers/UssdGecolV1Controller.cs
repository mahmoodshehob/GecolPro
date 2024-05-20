using GecolPro.Main.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Main.UssdService;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using GecolPro.Main.BusinessRules;
using ClassLibrary.Services;
using static GecolPro.Main.Models.MultiRequestUSSD;
using GecolPro.Main.Models;

namespace GecolPro.Main.Controllers
{
    [ApiController]
    //[Route("api/{Controller}")]
    public class UssdGecolV1Controller : ControllerBase
    {
        private static MultiRequest multiRequestRE = new MultiRequest();
        private static readonly string contentType = "text/xml";
        //private ServiceProcess.SendMessage sendMessage = new ServiceProcess.SendMessage();



        private readonly ApplicationDbContext _context;

        public UssdGecolV1Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/v1/En")]
        public async Task<ContentResult> PostEn()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequestUSSD.MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

               
                MultiResponseUSSD multiResponse = await UssdProcessV1.ServiceProcessing(multiRequest , "En");


                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = Responses.Resp(multiResponse),
                    StatusCode = 200
                };

              

                return response;
            }

        }

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/v1/Ar")]
        public async Task<ContentResult> PostAr()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

                MultiResponseUSSD multiResponse = await UssdProcessV1.ServiceProcessing(multiRequest, "Ar");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = Responses.Resp(multiResponse),
                    StatusCode = 200
                };

                return response;
            }
        }

    }
}

