
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Services.IServices;


//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using GecolPro.WebApi.BusinessRules;
using static GecolPro.Models.Models.MultiRequestUSSD;
using GecolPro.Models.Models;
using GecolPro.Services;
using GecolPro.WebApi.Interfaces;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]

    public class UssdGecolController : ControllerBase
    {

        private ILoggers _loggerG;
        private IUssdConverter _ussdConverter;
        private IResponses _responses;
        private IUssdProcessV1 _ussdProcess;



        private MultiRequest multiRequestRE = new MultiRequest();
        private readonly string contentType = "text/xml";
        //private ServiceProcess.SendMessage sendMessage = new ServiceProcess.SendMessage();


  




        public UssdGecolController(IUssdConverter ussdConverter, IResponses responses, ILoggers loggerG, IUssdProcessV1 ussdProcess)
        {
            _ussdConverter = ussdConverter;
            _responses = responses;
            _loggerG = loggerG;
            _ussdProcess = ussdProcess;
        }


        private async Task<ContentResult> GetResponseV1(string xmlContent, string lang)
        {
                        ContentResult response = new ContentResult();

            MultiRequestUSSD.MultiRequest multiRequest = await _ussdConverter.ConverterFaster(xmlContent);


            MultiResponseUSSD multiResponse = await _ussdProcess.ServiceProcessing(multiRequest, lang);

            await _loggerG.LogUssdTransAsync($"{xmlContent}");




            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                string respContetn = _responses.Resp(multiResponse);

                await _loggerG.LogUssdTransAsync($"{respContetn}");

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
                string respContetn = _responses.Fault_Response(multiResponse);

                await _loggerG.LogDcbTransAsync($"{respContetn}");

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
        [Route("api/[Controller]/creditVendReq/V1/En")]
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
        [Route("api/[Controller]/creditVendReq/V1/Ar")]
        public async Task<ContentResult> PostV1Ar()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequest multiRequest = await _ussdConverter.ConverterFaster(xmlContent);

                response = await GetResponseV1(xmlContent, "Ar");



                return response;
            }
        }
        #endregion

    }
}

