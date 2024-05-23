﻿using GecolPro.Main.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GecolPro.Main.UssdService;

//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using GecolPro.Main.BusinessRules;
using static ClassLibrary.Models.Models.MultiRequestUSSD;
using ClassLibrary.Models.Models;
using Newtonsoft.Json;

namespace GecolPro.Main.Controllers
{
    [ApiController]
    //[Route("api/{Controller}")]
    public class UssdGecolV2Controller : ControllerBase
    {
        private static MultiRequest multiRequestRE = new MultiRequest();
        private static readonly string contentType = "text/xml";
        //private ServiceProcess.SendMessage sendMessage = new ServiceProcess.SendMessage();



        private readonly ApplicationDbContext _context;

        public UssdGecolV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V2/En")]
        public async Task<ContentResult> PostEn()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequestUSSD.MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

               
                MultiResponseUSSD multiResponse = await UssdProcessV2.ServiceProcessing(multiRequest , "En");


                if (multiResponse.ResponseCode == 0 || multiResponse == null)
                {
                    response = new ContentResult
                    {
                        ContentType = contentType,
                        Content = Responses.Resp(multiResponse),
                        StatusCode = 200
                    };
                }

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = Responses.Fault_Response(multiResponse),
                    StatusCode = 400
                };


                return response;
            }

        }

        [HttpPost]
        [Consumes("text/xml")]
        [Route("api/{Controller}/creditVendReq/V2/Ar")]
        public async Task<ContentResult> PostAr()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string xmlContent = await reader.ReadToEndAsync();

                MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);

                MultiResponseUSSD multiResponse = await UssdProcessV1.ServiceProcessing(multiRequest, "Ar");

                if (multiResponse.ResponseCode == 0 || multiResponse == null)
                {
                    response = new ContentResult
                    {
                        ContentType = contentType,
                        Content = Responses.Resp(multiResponse),
                        StatusCode = 200
                    };
                }

                response = new ContentResult
                {
                    ContentType = contentType,
                    Content = Responses.Fault_Response(multiResponse),
                    StatusCode = 400
                };


                return response;
            }
        }




        [HttpGet]
        [Consumes("text/xml")]
        [Route("api/{Controller}/syncblacklist/V2/En")]
        public async Task<ContentResult> SyncBlackList()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ContentResult response = new ContentResult();

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string jsonFilePath = Path.Combine(baseDirectory, "BlackListMsisdn.json");
                
                if (!Directory.Exists(jsonFilePath))
                {
                    Directory.CreateDirectory(jsonFilePath);
                }

                //write not read
                var json = System.IO.File.ReadAllText(jsonFilePath);

                string[]? BlackList = JsonConvert.DeserializeObject<string[]>(json);
                return response;
            }

        }
    }
}

