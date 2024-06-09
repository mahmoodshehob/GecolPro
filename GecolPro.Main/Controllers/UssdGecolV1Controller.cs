﻿using GecolPro.Main.Data;
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
    public class UssdGecolV1Controller : ControllerBase
    {
        private static Loggers LoggerG = new Loggers();

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

                response = await GetResponse(xmlContent, "En");

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

                response = await GetResponse(xmlContent, "Ar");

                return response;
            }
        }


        private async Task<ContentResult> GetResponse(string xmlContent, string lang)
        {

            ContentResult response = new ContentResult();

            MultiRequestUSSD.MultiRequest multiRequest = await UssdConverter.ConverterFaster(xmlContent);


            MultiResponseUSSD multiResponse = await UssdProcessV1.ServiceProcessing(multiRequest, lang);

            await LoggerG.LogUssdTransAsync($"{xmlContent}");




            if (multiResponse.ResponseCode == 0 || multiResponse.ResponseCode == null)
            {
                string respContetn = Responses.Resp(multiResponse);

                await LoggerG.LogUssdTransAsync($"{OrganizeXmlString(respContetn)}");

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



        private async Task<string> OrganizeXmlString(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    " // Use four spaces for indentation
            };

            using (XmlWriter writer = XmlWriter.Create(stringBuilder, settings))
            {
                xmlDoc.WriteTo(writer);
            }

            string afterOrganizeXml = stringBuilder.ToString();

            return afterOrganizeXml;
        }
    }
}

