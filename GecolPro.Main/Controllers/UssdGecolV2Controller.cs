//using GecolPro.Main.Data;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;
//using System.Xml.Serialization;

//using GecolPro.Main.ServiceProcess;
//using GecolPro.Main.Models;


//using ClassLibrary.Services;




//namespace GecolPro.Main.Controllers
//{
//    [ApiController]
//    //[Route("api/{Controller}")]
//    public class UssdGecolV2Controller : ControllerBase
//    {
//        private ServiceProcess.IConvertReq ConvertReqToObject;
//        private ServiceProcess.ICreateXmlResp ConvertRsqToXml;
//        private ServiceProcess.ICreateXmlResp createXmlResp;

//        private Loggers logger = new Loggers();

//        private MultiRequestUSSD.MultiRequest multiRequest = new MultiRequestUSSD.MultiRequest();

//        private readonly string contentType = "text/xml";

//        //private readonly ApplicationDbContext _context;

//        //public UssdGecolV2Controller(ApplicationDbContext context)
//        //{
//        //    _context = context;
//        //}
//        [HttpPost]
//        [Consumes("text/xml")]
//        [Route("api/{Controller}/creditVendReq/v2/En")]
//        public async Task<ContentResult> PostEn()
//        {
//            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
//            {
//                ContentResult response = new ContentResult();

//                string xmlContent = await reader.ReadToEndAsync();

//                multiRequest = await ConvertReqToObject.ConverterFaster(xmlContent);

//                await logger.LogInfoAsync("This is an informational message.");

//                MultiResponseUSSD multiResponse = await UssdProcessV2.ServiceProcessing(multiRequest , "En");

//                response = new ContentResult
//                {
//                    ContentType = contentType,
//                    Content = createXmlResp.Resp(multiResponse),
//                    StatusCode = 200
//                };
//                return response;
//            }

//        }


//        [HttpPost]
//        [Consumes("text/xml")]
//        [Route("api/{Controller}/creditVendReq/v2/Ar")]
//        public async Task<ContentResult> PostAr()
//        {
//            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
//            {
//                ContentResult response = new ContentResult();

//                string xmlContent = await reader.ReadToEndAsync();

//                multiRequest = await ConvertReqToObject.ConverterFaster(xmlContent);

//                await logger.LogInfoAsync("This is an informational message.");

//                MultiResponseUSSD multiResponse = await UssdProcessV2.ServiceProcessing(multiRequest, "Ar");

//                response = new ContentResult
//                {
//                    ContentType = contentType,
//                    Content = createXmlResp.Resp(multiResponse),
//                    StatusCode = 200
//                };
//                return response;
//            }

//        }
//    }
//}

