using ClassLibrary.DCBSystem.Models;
using ClassLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClassLibrary.DCBSystem
{
    public class SoapServiceClient 
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly AuthHeader authHeader = new AuthHeader();

        private static Loggers LoggerG = new Loggers();

        public SoapServiceClient()
        {   
            _client.Timeout = TimeSpan.FromMilliseconds(5000);
        }

        public async Task<(string Responce, string StatusCode, Boolean state)> SendSoapRequest(string Body ,string SOAPAction)
        {
            try
            {
                string soapEnvelope = Body;
                var request = new HttpRequestMessage(HttpMethod.Post, authHeader.Url)
                {
                    Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", SOAPAction);


                await LoggerG.LogDcbTransAsync($"{Body}");

                HttpResponseMessage response = await _client.SendAsync(request);

                await LoggerG.LogDcbTransAsync($"{OrganizeXmlString(response.Content.ReadAsStringAsync().Result)}");


                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync(), response.StatusCode.ToString(), true);
                }
                else
                {
                    return (await response.Content.ReadAsStringAsync(), response.StatusCode.ToString(), false);

                    throw new Exception($"Error calling SOAP API: {response.StatusCode}");
                }
            }
            catch (TaskCanceledException ex1)
            {
                string message = ex1.Message;
                var InnerException = ex1.InnerException;
                string ExcLocation = ex1.StackTrace.Replace("\n", "|");

                return ("timeout", ex1.Message, false);

            }
            catch (Exception ex) 
            {
                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");

                return ("BillingSystemUnderMaintenance", ex.Message, false);

            }
        }


        private string OrganizeXmlString(string xml)
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


