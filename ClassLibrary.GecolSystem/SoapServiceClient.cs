using ClassLibrary.GecolSystem.Models;
using ClassLibrary.Services;
using System.Text;


namespace ClassLibrary.GecolSystem
{
    public class SoapServiceClient
    {
        private readonly HttpClient _client;
        private readonly AuthCred authCred;
        //private Logger logger;
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

                HttpResponseMessage response = await _client.SendAsync(request);


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
    }
}


