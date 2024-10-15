using GecolPro.Main.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Main.UssdService;

//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using GecolPro.Main.BusinessRules;
using static ClassLibrary.Models.Models.MultiRequestUSSD;
using ClassLibrary.Models.Models;
using ClassLibrary.Services;

namespace GecolPro.Main.Controllers
{
    [ApiController]
    //[Route("api/{Controller}")]
    public class UssdGecolController : ControllerBase
    {
        private static Loggers LoggerG = new Loggers();

        private static MultiRequest multiRequestRE = new MultiRequest();
        private static readonly string contentType = "text/xml";
        //private ServiceProcess.SendMessage sendMessage = new ServiceProcess.SendMessage();



        private readonly ApplicationDbContext _context;

        public UssdGecolController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<ContentResult> GetResponseV1(string xmlContent, string lang)
        {
                        ContentResult response = new ContentResult();

            MultiRequestUSSD.MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);


            MultiResponseUSSD multiResponse = await UssdProcessV1.ServiceProcessing(multiRequest, lang);

            await LoggerG.LogUssdTransAsync($"{xmlContent}");




            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                string respContetn = Responses.Resp(multiResponse);

                await LoggerG.LogUssdTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 200
                };
                return response;

            }
            else
            {
                string respContetn = Responses.Fault_Response(multiResponse);

                await LoggerG.LogDcbTransAsync($"{respContetn}");

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = respContetn,
                    StatusCode = 400
                };
                return response;
            }
        }




        #region API Region 

        // Version 1

        //English

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V1/En")]
        public async Task<ContentResult> PostV1En()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                response = await GetResponseV1(xmlContent, "En");

                return response;
            }

        }

        //Arabic

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V1/Ar")]
        public async Task<ContentResult> PostV1Ar()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

                response = await GetResponseV1(xmlContent, "Ar");



                return response;
            }
        }
        #endregion

    }
}

