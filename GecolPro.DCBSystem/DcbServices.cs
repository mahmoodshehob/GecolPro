using GecolPro.Models.DCB;
//using GecolPro.Models.Gecol;
using GecolPro.Models.Models;
using GecolPro.Services;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Xml;

namespace GecolPro.DCBSystem
{
    public class DcbServices : IDcbServices
    {

        private readonly IDcbCreateResponse _createResponse;
        private readonly IDcbCreateXml _createXml;
        private readonly AuthHeader _authHeader;
        private static Loggers LoggerG = new Loggers();


        // Constructor with IOptions<AuthCred> for dependency injection
        public DcbServices(IConfiguration config, IDcbCreateResponse createResponse, IDcbCreateXml createXml)
        {
            _authHeader = new AuthHeader();
            config.GetSection("AuthHeaderOfDCB").Bind(_authHeader);
            _createResponse = createResponse ?? throw new ArgumentNullException(nameof(createResponse));
            _createXml = createXml ?? throw new ArgumentNullException(nameof(createXml));
        }


        private class SoapAction
        {
            public const string QryUserBasicBal = "QryUserBasicBal";
            public const string DirectDebitUnit = "DirectDebitUnit";
            public const string DebitRollback = "DebitRollback";
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

            return stringBuilder.ToString();
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

                var soapRsp = await SendSoapRequest(body, SoapAction.QryUserBasicBal);

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

        public async Task<Result<SuccessResponseQryUserBasicBal, FailureResponse>> QryUserBasicBalOpX(string msisdn)
        {
            try
            {

                QryUserBasicBalSoap qryUserBasicBalSoap = new()
                {
                    MSISDN = msisdn,
                };

                var body = _createXml.CreateXmlQryUserBal(qryUserBasicBalSoap);

                var soapRsp = await SendSoapRequest(body, SoapAction.QryUserBasicBal);


                if (soapRsp.IsSuccessStatusCode)
                {


                    QryUserBasicBalRsp qryUserBasicBalRsp = await _createResponse.ToQryUserBasicRsp(soapRsp.Response);

                    qryUserBasicBalRsp.CommentsOrBalance = (int.Parse(qryUserBasicBalRsp.BalanceDto.BalanceValue) / 100000).ToString();

                    SuccessResponseQryUserBasicBal successResponseResult = new SuccessResponseQryUserBasicBal()
                    {
                        Response = qryUserBasicBalRsp,
                        StatusCode = soapRsp.StatusCode,
                        IsSuccessStatusCode = true
                    };

                    return Result<SuccessResponseQryUserBasicBal, FailureResponse>.SuccessResult(successResponseResult);



                }
                else if (soapRsp.StatusCode.ToLower() != "timeout")
                {
                    var faultModel = await _createResponse.ToFaultRsp(soapRsp.Response);

                    FailureResponse failureResponse = new FailureResponse()
                    {
                        Failure = faultModel.FaultString,
                        StatusCode = faultModel.FaultCode,
                        IsSuccessStatusCode = false
                    };

                    return Result<SuccessResponseQryUserBasicBal, FailureResponse>.FailureResult(failureResponse);

                }
                else
                {
                    FailureResponse failureResponse = new FailureResponse()
                    {
                        Failure = soapRsp.Response,
                        StatusCode = "timeout",
                        IsSuccessStatusCode = false
                    };

                    return Result<SuccessResponseQryUserBasicBal, FailureResponse>.FailureResult(failureResponse);


                }

            }
            catch (Exception ex)
            {
                LogException(ex);

                FailureResponse failureResponse = new FailureResponse()
                {
                    Failure = "Fault",
                    StatusCode = "ex.Message",
                    IsSuccessStatusCode = false
                };

                return Result<SuccessResponseQryUserBasicBal, FailureResponse>.FailureResult(failureResponse);
            }
        }

        public async Task<Result<SuccessResponseDirectDebit, FailureResponse>> DirectDebitUnitOp(string conversationId, string msisdn, int amount)
        {
            var statusCode = "";
            string? RespDesc;
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

                var soapRsp = await SendSoapRequest(body, SoapAction.DirectDebitUnit);

                RespDesc = soapRsp.StatusCode;


                if (soapRsp.IsSuccessStatusCode)
                {
                    DirectDebitUnitRsp directDebitUnit = await _createResponse.ToDirectDebitUnitCRsp(soapRsp.Response);

                    SuccessResponseDirectDebit successResponseResult = new SuccessResponseDirectDebit()
                    {
                        Response = directDebitUnit,
                        StatusCode = soapRsp.StatusCode,
                        IsSuccessStatusCode = true
                    };

                    return Result<SuccessResponseDirectDebit, FailureResponse>.SuccessResult(successResponseResult);

                }
                else if (soapRsp.StatusCode.ToLower() != "timeout")
                {
                    var faultModel = await _createResponse.ToFaultRsp(soapRsp.Response);

                    FailureResponse failureResponse = new FailureResponse()
                    {
                        Failure = faultModel.FaultString,
                        StatusCode = faultModel.FaultCode,
                        IsSuccessStatusCode = false
                    };

                    return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(failureResponse);

                }
                else
                {

                    statusCode = "timeout";


                    FailureResponse failureResponse = new FailureResponse()
                    {
                        Failure = soapRsp.Response,
                        StatusCode = "timeout",
                        IsSuccessStatusCode = false
                    };

                    return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(failureResponse);


                }

            }
            catch (Exception ex)
            {
                LogException(ex);

                FailureResponse failureResponse = new FailureResponse()
                {
                    Failure = "Fault",
                    StatusCode = "ex.Message",
                    IsSuccessStatusCode = false
                };

                return Result<SuccessResponseDirectDebit, FailureResponse>.FailureResult(failureResponse);
            }
        }

        public async Task<DcbSystemResponse> DebitRollbackOp(string conversationId, string transactionId, string msisdn, int amount)
        {
            var statusCode = "";
            try
            {
                DebitRollbackReqSoap debitRollbackReq = new()
                {
                    ServiceName = "Gecol",
                    TransactionID = transactionId,
                    ConversationID = conversationId,
                    DestinationAddress = msisdn,
                    Amount = amount * 1000
                };

                var body = _createXml.CreateXmlDebitRollback(debitRollbackReq);

                var soapRsp = await SendSoapRequest(body, SoapAction.DebitRollback);

                statusCode = soapRsp.StatusCode;


                if (soapRsp.IsSuccessStatusCode)
                {
                    var debitRollbackRsp = await _createResponse.ToDebitRollbackRsp(soapRsp.Response);
                    return new DcbSystemResponse(debitRollbackRsp.TransactionID, statusCode, soapRsp.IsSuccessStatusCode);
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

        private async Task<DcbSystemResponse> SendSoapRequest(string Body, string soapAction)
        {
            string? statusCode ="";
            try
            {
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };


                var request = new HttpRequestMessage(HttpMethod.Post, _authHeader.Url)
                {
                    Content = new StringContent(Body, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", soapAction);

                await LoggerG.LogDcbTransAsync($"{Body}");

                var response = await client.SendAsync(request);

                await LoggerG.LogDcbTransAsync($"{OrganizeXmlString(response.Content.ReadAsStringAsync().Result)}");

                statusCode = response.StatusCode.ToString();

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
            catch (TaskCanceledException ex)
            {
                LogException(ex);
                return new DcbSystemResponse(ex.Message, "timeout", false);

            }
            catch (HttpRequestException ex)
            {
                LogException(ex);
                return new DcbSystemResponse(ex.Message, ex.GetType().Name, false);

            }
            catch (Exception ex)
            {
                LogException(ex);
                var lll = ex.GetType().Name;
                return new DcbSystemResponse(ex.Message, statusCode, false);

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
        //public Task<DcbSystemResponse> QryUserBasicBalOp(string msisdn);

        public Task<Result<SuccessResponseQryUserBasicBal, FailureResponse>> QryUserBasicBalOpX(string msisdn);

        public Task<Result<SuccessResponseDirectDebit, FailureResponse>> DirectDebitUnitOp(string conversationId, string msisdn, int amount);

        public Task<DcbSystemResponse> DebitRollbackOp(string conversationId, string transactionId, string msisdn, int amount);
    }
}
