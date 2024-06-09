using ClassLibrary.GecolSystem.Models;
using ClassLibrary.Services;
using System.Text;
using System.Xml;


namespace ClassLibrary.GecolSystem
{
    public class SoapServiceClient
    {
        private readonly HttpClient _client;
        private readonly AuthCred authCred;
        private static Loggers LoggerG = new Loggers();
        //private static IniFiles readIniFile = new IniFiles();
        //private static Dictionary<string, string> dictionary = readIniFile.Read("GECOL_Section");




        public SoapServiceClient()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromMilliseconds(10000);

            authCred = new AuthCred();
            //logger = new Logger();
            //{
            //    Url = dictionary

            //};
        }

        public async Task<(string Responce, string StatusCode, Boolean state)> SendSoapRequest(string Body ,string SOAPAction)
        {
        
            string statusCode = null;
            
            try
            {
                string soapEnvelope = Body;
                var request = new HttpRequestMessage(HttpMethod.Post, authCred.Url)
                {
                    Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                };


                await LoggerG.LogGecolTransAsync($"{Body}");

                HttpResponseMessage response = await _client.SendAsync(request);

                await LoggerG.LogGecolTransAsync($"{OrganizeXmlString(response.Content.ReadAsStringAsync().Result)}");


                statusCode = response.StatusCode.ToString();
                
                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync(), statusCode, true);
                }
                else
                {
                    return (await response.Content.ReadAsStringAsync(), statusCode, false);

                    throw new Exception($"Error calling SOAP API: {response.StatusCode}");
                }
            }
            catch (Exception ex) 
            {
                string ErrorId = DateTime.Now.ToString("yyyyMMddTHHmmss");
                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");
                //logger.LogErrorAsync(ErrorId + "|" + SOAPAction + "|" + message + "|" + InnerException + "|" + ExcLocation);

                return (message, statusCode, false);
            }
        }


        private static string OrganizeXmlString(string xml)
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

            return stringBuilder.ToString();
        }


    }
}


