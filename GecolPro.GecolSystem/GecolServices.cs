using System;
using System.Text;
using System.Text.Json;
using System.Xml;

using System.Xml.Linq;
using System.Xml.Serialization;

using GecolPro.GecolSystem.Models;
using GecolPro.Services;

using GecolPro.Models.Models;


namespace GecolPro.GecolSystem
{
    public class GecolServices : IGecolServices
    {
        //private readonly SoapServiceClient _soapServiceClient = new();
        private readonly ICreateResponse _createResponse = new XmlServices();
        private readonly ICreateXml _createXml = new XmlServices();
        private static Loggers LoggerG = new Loggers();


        private enum SoapActionEnum
        {
            LoginReq,
            CreditVendReq,
            ConfirmCustomerReq
        }


        public async Task<GecolSystemResponse> LoginReqOp()
        {

            try
            {
                var body = OrganizeXmlString(_createXml.CreateXmlLoginRequest());


                var soapRsp = await SendSoapRequest(body, SoapActionEnum.LoginReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
                    //  here the response of Success Case
                    var accountWallet = "Exception";

                    try
                    {
                        var loginRsp = await _createResponse.ToLoginRsp(soapRsp.Response);

                        await Console.Out.WriteLineAsync(
                            loginRsp.ID + "\n" +
                            loginRsp.TID + "\n" +
                            loginRsp.CDUID + "\n" +
                            loginRsp.CDUName + "\n" +
                            loginRsp.CDUBalance + "\n" +
                            loginRsp.MinVendAmt + "\n" +
                            loginRsp.MaxVendAmt + "\n" +
                            loginRsp.LoginTime + "\n");

                        accountWallet = loginRsp.CDUBalance.ToString();
                        // Success Case;

                        //return (SoapRsp.Responce, SoapRsp.StatusCode, SoapRsp.state);
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);


                        accountWallet = "Exception";
                    }

                    return new GecolSystemResponse(accountWallet, soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);
                }
                else
                {
                    return new GecolSystemResponse(await FailedCase(soapRsp.Response), soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);
                }
            }
            catch (Exception ex)
            {

                LogException(ex);

                return new GecolSystemResponse("Fault", ex.Message, false);
            }
        }

        public async Task<GecolSystemResponse> ConfirmCustomerOp(string meterNumber)
        {
            try
            {

                var body = _createXml.CreateXmlCustomerRequest(meterNumber);

                var soapRsp = await SendSoapRequest(body, SoapActionEnum.ConfirmCustomerReq.ToString());

                if (!soapRsp.IsSuccessStatusCode) return new GecolSystemResponse(await FailedCase(soapRsp.Response), soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);
                string meterExisting;
                try
                {
                    //  var  confirmCustomerRsp = await GecolConvertRsp.Converte(SoapRsp.Responce);
                    meterExisting = "Meter Exist";
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    meterExisting = "Exception";

                }

                return new GecolSystemResponse(meterExisting, soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);

            }
            catch (Exception ex)
            {
                LogException(ex);

                return new GecolSystemResponse("Fault", ex.Message, false);
            }
        }

        public async Task<GecolSystemResponse> CreditVendOp(string meterNumber, string uniqeNumber, int purchaseValue)
        {
            CreditVendRespBody.CreditVendResp creditVendResp = new CreditVendRespBody.CreditVendResp();

            try
            {

                var body = _createXml.CreateXmlCreditVendRequest(meterNumber, uniqeNumber, purchaseValue);

                var soapRsp = await SendSoapRequest(body, SoapActionEnum.CreditVendReq.ToString());

                if (!soapRsp.IsSuccessStatusCode) return new GecolSystemResponse(await FailedCase(soapRsp.Response), soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);




                string? creditToken;
                try
                {
                    // Response in Json formate
                    creditToken = JsonSerializer.Serialize(await _createResponse.ToCreditVendCRsp(soapRsp.Response));

                
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    creditToken = "Exception";
                }

                return new GecolSystemResponse(creditToken!, soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);

            }
            catch (Exception ex)
            {
                LogException(ex);
                return new GecolSystemResponse("Fault", ex.Message, false);
            }
        }

        private async Task<string> FailedCase(string failedXmlRespons)
        {
            try
            {
                var faultModel = await _createResponse.ToFaultRsp(failedXmlRespons);
                return faultModel.FaultCode;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return ("Fault");
            }
        }

        private static async Task<GecolSystemResponse> SendSoapRequest(string body, string soapAction)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            var authCred = new AuthCred();
            var statusCode = "";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, authCred.Url)
                {
                    Content = new StringContent(body, Encoding.UTF8, "text/xml")
                };

                //string TransID = DateTime.Now.ToString("ffff");

                await LoggerG.LogGecolTransAsync($"{body}");

                var response = await client.SendAsync(request);

                await LoggerG.LogGecolTransAsync($"{OrganizeXmlString(response.Content.ReadAsStringAsync().Result)}");



                statusCode = response.StatusCode.ToString();
                var result = new GecolSystemResponse(await response.Content.ReadAsStringAsync(), statusCode,
                    response.IsSuccessStatusCode);

                return result;

            }
            catch (Exception ex)
            {
                LogException(ex);

                return new GecolSystemResponse(ex.Message, statusCode, false);
            }
        }

        private static void LogException(Exception ex)
        {
            var errorId = DateTime.Now.ToString("yyyyMMddTHHmmss");
            var message = ex.Message;
            var innerException = ex.InnerException;
            var excLocation = ex.StackTrace!.Replace("\n", "|");

            // Log exception details
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

            string afterOrganizeXml = stringBuilder.ToString();

            return afterOrganizeXml;
        }

    }

    public interface IGecolServices
    {
        public Task<GecolSystemResponse> LoginReqOp();
        public Task<GecolSystemResponse> ConfirmCustomerOp(string meterNumber);
        public  Task<GecolSystemResponse> CreditVendOp(string meterNumber, string uniqeNumber, int purchaseValue);
    }
}