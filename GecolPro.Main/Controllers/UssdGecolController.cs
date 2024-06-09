using GecolPro.Main.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Main.UssdService;

//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using GecolPro.Main.BusinessRules;
using static ClassLibrary.Models.Models.MultiRequestUSSD;
using ClassLibrary.Models.Models;
using Newtonsoft.Json;
using ClassLibrary.Services;
using System.Xml;

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

        private async Task<ContentResult> GetResponseV2(string xmlContent, string lang)
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



        // Version 1

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/v1/En")]
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

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/v1/Ar")]
        public async Task<ContentResult> PostV1Ar()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                response = await GetResponseV1(xmlContent, "Ar");

                return response;
            }
        }




        // Version 2

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V2/En")]
        public async Task<ContentResult> PostV2En()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                response = await GetResponseV2(xmlContent, "En");

                return response;
            }

        }

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V2/Ar")]
        public async Task<ContentResult> PostV2Ar()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

                response = await GetResponseV2(xmlContent, "Ar");



                return response;
            }
        }


    }
}

