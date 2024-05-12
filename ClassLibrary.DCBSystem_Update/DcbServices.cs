using System.Text;
using ClassLibrary.DCBSystem_Update.Models;

namespace ClassLibrary.DCBSystem_Update
{
    public class DcbServices
    {

        private readonly XmlServices.ICreateResponse _createResponse = new XmlServices();
        private readonly XmlServices.ICreateXml _createXml = new XmlServices();



        //private static QryUserBasicBalRsp qryUserBasicBalRsp = new QryUserBasicBalRsp();

        //private static DirectDebitUnitRsp directDebitUnitRsp = new DirectDebitUnitRsp();

        //private static string FaultString = "";

        private enum SoapAction
        {
            QryUserBasicBal,
            DirectDebitUnit,
            DebitRollback
        }


        public async Task<DcbSystemResponse> QryUserBasicBalOp(string msisdn)
        {
            var statusCode = "";
            try
            {


                QryUserBasicBalRsp qryUserBasicBalRsp = new();
                QryUserBasicBalSoap qryUserBasicBalSoap = new()
                {
                    MSISDN = msisdn,
                };

                qryUserBasicBalRsp.MSISDN = msisdn;

                var body = _createXml.CreateXmlQryUserBal(qryUserBasicBalSoap);

                var soapRsp = await SendSoapRequest(body, SoapAction.QryUserBasicBal.ToString());

                statusCode = soapRsp.StatusCode;

                if (soapRsp.IsSuccessStatusCode)
                {
                    qryUserBasicBalRsp = await _createResponse.ToQryUserBasicRsp(soapRsp.Response);

                    var balance = (int.Parse(qryUserBasicBalRsp.BalanceDto.BalanceValue) / 100000).ToString();

                    return new DcbSystemResponse(balance, statusCode, true);
                }

                string faultCode;
                if (soapRsp.Response.ToLower() != "timeout")
                {
                    faultCode = await FailedCase(soapRsp.Response);
                }
                else
                {
                    faultCode = "timeout";
                }

                return new DcbSystemResponse(faultCode, statusCode, false);
            }
            catch (Exception e)
            {
                LogException(e);
                return new DcbSystemResponse(e.Message, statusCode, false);
            }
        }

        public async Task<DcbSystemResponse> DirectDebitUnitOp(string conversationId, string msisdn, int amount)
        {
            var statusCode = "";
            try
            {
                DirectDebitUnitReqSoap directDebitUnitReq = new()
                {
                    ServiceName = "Gecol",

                    ConversationID = conversationId,
                    DestinationAddress = msisdn,
                    Amount = amount * 1000
                };

                var body = _createXml.CreateXmlDirectDebitUnit(directDebitUnitReq);

                var soapRsp = await SendSoapRequest(body, SoapAction.DirectDebitUnit.ToString());

                statusCode = soapRsp.StatusCode;


                if (soapRsp.IsSuccessStatusCode)
                {
                    var directDebitUnitRsp = await _createResponse.ToDirectDebitUnitCRsp(soapRsp.Response);
                    return new DcbSystemResponse(directDebitUnitRsp.TransactionID, statusCode, soapRsp.IsSuccessStatusCode);
                }

                string? faultCode;
                if (soapRsp.Response.ToLower() != "timeout")
                {

                    faultCode = await FailedCase(soapRsp.Response);

                }
                else
                {

                    faultCode = "timeout";

                }
                return new DcbSystemResponse(faultCode, statusCode, soapRsp.IsSuccessStatusCode);
            }
            catch (Exception e)
            {
                LogException(e);
                return new DcbSystemResponse(e.Message, statusCode, false);
            }
        }

        public async Task<string?> FailedCase(string failedXmlRespons)
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

        private static async Task<DcbSystemResponse> SendSoapRequest(string body, string soapAction)
        {
            try
            {
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(1000)
                };
                var authHeader = new AuthHeader();

                var request = new HttpRequestMessage(HttpMethod.Post, authHeader.Url)
                {
                    Content = new StringContent(body, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", soapAction);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new DcbSystemResponse(await response.Content.ReadAsStringAsync(), response.StatusCode.ToString(), true);
                }
                else
                {
                    return new DcbSystemResponse(await response.Content.ReadAsStringAsync(), response.StatusCode.ToString(), false);

                    throw new Exception($"Error calling SOAP API: {response.StatusCode}");
                }
            }
            catch (TaskCanceledException ex1)
            {
                LogException(ex1);
                return new DcbSystemResponse("timeout", ex1.Message, false);

            }
            catch (Exception ex)
            {
                LogException(ex);

                return new DcbSystemResponse("BillingSystemUnderMaintenance", ex.Message, false);

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

    }
    public interface IDcbServices
    {
        public Task<DcbSystemResponse> QryUserBasicBalOp(string msisdn);
        public Task<DcbSystemResponse> DirectDebitUnitOp(string conversationId, string msisdn, int amount);
    }
}
