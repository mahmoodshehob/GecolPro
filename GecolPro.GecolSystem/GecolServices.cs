using System.Text;
using System.Text.Json;
using System.Xml;
using GecolPro.Models.Gecol;
using GecolPro.Services;
using GecolPro.Models.Models;
using Microsoft.Extensions.Configuration;
using static GecolPro.Models.Gecol.LoginRspXml;
using System.Windows.Markup;

namespace GecolPro.GecolSystem
{
    public class GecolServices : IGecolServices
    {
        private readonly IGecolCreateResponse _createResponse;
        private readonly IGecolCreateXml _createXml;
        private readonly AuthCred _authCred;
        private static Loggers LoggerG = new Loggers();

        public GecolServices(IConfiguration config, IGecolCreateResponse createResponse, IGecolCreateXml createXml)
        {
            _authCred = new AuthCred();
            config.GetSection("AuthHeaderOfGecol").Bind(_authCred);
            _createResponse = createResponse ?? throw new ArgumentNullException(nameof(createResponse));
            _createXml = createXml ?? throw new ArgumentNullException(nameof(createXml));
        }

        private enum SoapAction
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

                var soapRsp = await SendSoapRequest(body, SoapAction.LoginReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
                    //  here the response of Success Case
                    var accountWallet = "Exception"; //?

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

                var soapRsp = await SendSoapRequest(body, SoapAction.ConfirmCustomerReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {

                    var MeterDetails = await _createResponse.ToCreateXmlCustomerCRsp(soapRsp.Response);

                    string meterExisting;
                    try
                    {
                        meterExisting = "Meter Exist";
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                        meterExisting = "Exception";

                    }

                    return new GecolSystemResponse(meterExisting, soapRsp.StatusCode, soapRsp.IsSuccessStatusCode);
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

        public async Task<GecolSystemResponse> CreditVendOp(string meterNumber, string uniqeNumber, int purchaseValue)
        {
            CreditVendRespBody.CreditVendResp creditVendResp = new CreditVendRespBody.CreditVendResp();

            try
            {

                var body = _createXml.CreateXmlCreditVendRequest(meterNumber, uniqeNumber, purchaseValue);

                var soapRsp = await SendSoapRequest(body, SoapAction.CreditVendReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
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




        public async Task<Result<SuccessResponseLogin, FailureResponse>> LoginReqOpx()
        {
            try
            {
                var body = OrganizeXmlString(_createXml.CreateXmlLoginRequest());

                var soapRsp = await SendSoapRequest(body, SoapAction.LoginReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
                    //  here the response of Success Case
                    try
                    {
                        var loginRsp = await _createResponse.ToLoginRsp(soapRsp.Response);

                        SuccessResponseLogin successResponseResult = new SuccessResponseLogin()
                        {
                            Response = loginRsp,
                            StatusCode = soapRsp.StatusCode,
                            IsSuccessStatusCode = true
                        };
                        return Result<SuccessResponseLogin, FailureResponse>.SuccessResult(successResponseResult);

                    }
                    catch (Exception ex)
                    {
                        LogException(ex);

                        FailureResponse failureResponse = new FailureResponse()
                        {
                            Failure = ex.Message,
                            StatusCode = "Excp99",
                            IsSuccessStatusCode = false
                        };

                        return Result<SuccessResponseLogin, FailureResponse>.FailureResult(failureResponse);


                    }

                }
                else
                {
                    FaultModel.xmlvendFaultRespFault failuerRsp = await _createResponse.ToFaultRsp(soapRsp.Response);

                    FailureResponse failureResponse = new FailureResponse()
                    {

                        Failure = failuerRsp.Desc,
                        StatusCode = failuerRsp.FaultCode,
                        IsSuccessStatusCode = false
                    };


                    return Result<SuccessResponseLogin, FailureResponse>.FailureResult(failureResponse);
                }
            }
            catch (Exception ex)
            {

                LogException(ex);

                //return new GecolSystemResponse("Fault", ex.Message, false);

                FailureResponse failureResponse = new FailureResponse()
                {
                    Failure = "Fault",
                    StatusCode = "ex.Message",
                    IsSuccessStatusCode = false
                };

                return Result<SuccessResponseLogin, FailureResponse>.FailureResult(failureResponse);

            }
        }

        public async Task<Result<SuccessResponseConfirmCustomer, FailureResponse>> ConfirmCustomerOpx(string meterNumber)
        {
            try
            {
                var body = _createXml.CreateXmlCustomerRequest(meterNumber);

                var soapRsp = await SendSoapRequest(body, SoapAction.ConfirmCustomerReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
                    try
                    {
                        var MeterDetails = await _createResponse.ToCreateXmlCustomerCRsp(soapRsp.Response);


                        SuccessResponseConfirmCustomer successResponseResult = new SuccessResponseConfirmCustomer()
                        {
                            Response = MeterDetails,
                            StatusCode = soapRsp.StatusCode,
                            IsSuccessStatusCode = true
                        };
                        return Result<SuccessResponseConfirmCustomer, FailureResponse>.SuccessResult(successResponseResult);

                    }
                    catch (Exception ex)
                    {
                        LogException(ex);

                        FailureResponse failureResponse = new FailureResponse()
                        {
                            Failure = ex.Message,
                            StatusCode = "Excp99",
                            IsSuccessStatusCode = false
                        };

                        return Result<SuccessResponseConfirmCustomer, FailureResponse>.FailureResult(failureResponse);


                    }

                }
                else
                {
                    FaultModel.xmlvendFaultRespFault failuerRsp = await _createResponse.ToFaultRsp(soapRsp.Response);

                    FailureResponse failureResponse = new FailureResponse()
                    {

                        Failure = failuerRsp.Desc,
                        StatusCode = failuerRsp.FaultCode,
                        IsSuccessStatusCode = false
                    };


                    return Result<SuccessResponseConfirmCustomer, FailureResponse>.FailureResult(failureResponse);
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

                return Result<SuccessResponseConfirmCustomer, FailureResponse>.FailureResult(failureResponse);

            }
        }

        public async Task<Result<SuccessResponseCreditVend, FailureResponse>> CreditVendOpx(string meterNumber, string uniqeNumber, int purchaseValue)
        {
            CreditVendRespBody.CreditVendResp creditVendResp = new CreditVendRespBody.CreditVendResp();

            try
            {

                var body = _createXml.CreateXmlCreditVendRequest(meterNumber, uniqeNumber, purchaseValue);

                var soapRsp = await SendSoapRequest(body, SoapAction.CreditVendReq.ToString());

                if (soapRsp.IsSuccessStatusCode)
                {
                    try
                    {
                        var invoice = await _createResponse.ToCreditVendCRsp(soapRsp.Response);

                        SuccessResponseCreditVend successResponseResult = new SuccessResponseCreditVend()
                        {
                            Response = invoice,
                            StatusCode = soapRsp.StatusCode,
                            IsSuccessStatusCode = true
                        };
                        return Result<SuccessResponseCreditVend, FailureResponse>.SuccessResult(successResponseResult);


                    }
                    catch (Exception ex)
                    {
                        LogException(ex);

                        FailureResponse failureResponse = new FailureResponse()
                        {
                            Failure = ex.Message,
                            StatusCode = "Excp99",
                            IsSuccessStatusCode = false
                        };

                        return Result<SuccessResponseCreditVend, FailureResponse>.FailureResult(failureResponse);
                    }

                }
                else
                {

                    FaultModel.xmlvendFaultRespFault failuerRsp =await _createResponse.ToFaultRsp(soapRsp.Response);

                    FailureResponse failureResponse = new FailureResponse()
                    {

                        Failure = failuerRsp.Desc,
                        StatusCode = failuerRsp.FaultCode,
                        IsSuccessStatusCode = false
                    };

                    return Result<SuccessResponseCreditVend, FailureResponse>.FailureResult(failureResponse);
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

                return Result<SuccessResponseCreditVend, FailureResponse>.FailureResult(failureResponse);

            }
        }



        //



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

        private async Task<GecolSystemResponse> SendSoapRequest(string body, string soapAction)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            var authCred = _authCred;
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

                return new GecolSystemResponse(await response.Content.ReadAsStringAsync(), statusCode,
                    response.IsSuccessStatusCode);

            }
            catch (Exception ex)
            {
                LogException(ex);

                return new GecolSystemResponse(ex.Message, statusCode, false);
            }
        }

        private void LogException(Exception ex)
        {
            var errorId = DateTime.Now.ToString("yyyyMMddTHHmmss");
            var message = ex.Message;
            var innerException = ex.InnerException;
            var excLocation = ex.StackTrace!.Replace("\n", "|");

            // Log exception details
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
    public interface IGecolServices
    {
        //public Task<GecolSystemResponse> LoginReqOp();
        
        //public Task<GecolSystemResponse> ConfirmCustomerOp(string meterNumber);
       
        //public  Task<GecolSystemResponse> CreditVendOp(string meterNumber, string uniqeNumber, int purchaseValue);



        public Task<Result<SuccessResponseLogin, FailureResponse>> LoginReqOpx();

        public Task<Result<SuccessResponseConfirmCustomer, FailureResponse>> ConfirmCustomerOpx(string meterNumber);

        public Task<Result<SuccessResponseCreditVend, FailureResponse>> CreditVendOpx(string meterNumber, string uniqeNumber, int purchaseValue);
    }
}