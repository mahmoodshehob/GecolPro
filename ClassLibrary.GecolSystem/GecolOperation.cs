using ClassLibrary.GecolSystem.Models;
using Newtonsoft.Json.Linq;
using System;


using static ClassLibrary.GecolSystem.Models.FaultModel;
using static ClassLibrary.GecolSystem.Models.LoginRspXml;

namespace ClassLibrary.GecolSystem
{
    public class GecolOperation
    {
        private static SoapServiceClient soapServiceClient = new SoapServiceClient();

        private static LoginRsp loginRsp = new LoginRsp();

        private static ConfirmCustomerRsp confirmCustomerRsp = new ConfirmCustomerRsp();


        private enum SOAPAction
        {
            LoginReq,
            CreditVendReq,
            ConfirmCustomerReq
        }





        public async static Task<(string Response, string StatusCode, Boolean Status)> LoginReqOp()
        {

            try
            {

                string Body = GecolCreateXml.LoginReqCreateXml.CreateSoapBody();

                (string Responce, string StatusCode, Boolean state) SoapRsp = await soapServiceClient.SendSoapRequest(Body, SOAPAction.LoginReq.ToString());

                if (SoapRsp.state == true)
                {
                    //  here the response of Success Case
                    string AccountWallet = "Exception";
                    
                    try
                    {
                        loginRsp = await GecolConvertRsp.ConvLoginRsp.Converte(SoapRsp.Responce);

                        await Console.Out.WriteLineAsync(
                            loginRsp.ID + "\n" +
                            loginRsp.TID + "\n" +
                            loginRsp.CDUID + "\n" +
                            loginRsp.CDUName + "\n" +
                            loginRsp.CDUBalance + "\n" +
                            loginRsp.MinVendAmt + "\n" +
                            loginRsp.MaxVendAmt + "\n" +
                            loginRsp.LoginTime + "\n");

                        AccountWallet = loginRsp.CDUBalance.ToString();
                        // Success Case;

                        //return (SoapRsp.Responce, SoapRsp.StatusCode, SoapRsp.state);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        var InnerException = ex.InnerException;
                        string ExcLocation = ex.StackTrace.Replace("\n", "|");


                        AccountWallet = "Exception";
                    }

                    return (AccountWallet, SoapRsp.StatusCode, SoapRsp.state);
                }
                else
                {
                    return (await FailedCase(SoapRsp.Responce), SoapRsp.StatusCode, SoapRsp.state);
                }
            }
            catch (Exception ex)
            {

                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");

                return ("Fault", message, false);
            }
        }


        public async static Task<(string Response, string StatusCode, Boolean Status)> ConfirmCustomerOp(string MeterNumber)
        {
            try
            {

                string Body = GecolCreateXml.ConfirmCustomerReqCreateXml.CreateSoapBody(MeterNumber);

                (string Responce, string StatusCode, Boolean state) SoapRsp = await soapServiceClient.SendSoapRequest(Body, SOAPAction.ConfirmCustomerReq.ToString());

                if (SoapRsp.state == true)
                {
                    string MeterExisting;
                    try
                    {
                        //  var  confirmCustomerRsp = await GecolConvertRsp.Converte(SoapRsp.Responce);
                        MeterExisting = "Meter Exist";
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        var InnerException = ex.InnerException;
                        string ExcLocation = ex.StackTrace.Replace("\n", "|");

                        MeterExisting = "Exception";

                    }

                    return (MeterExisting, SoapRsp.StatusCode, SoapRsp.state);
                }
                else
                {
                    return (await FailedCase(SoapRsp.Responce), SoapRsp.StatusCode, SoapRsp.state);

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");

                return ("Fault", message, false);
            }
        }


        public async static Task<(string Response, string StatusCode, Boolean Status)> CreditVendOp(string MeterNumber,string UniqeNumber , int PurchaseValue)
        {
            try
            {

                string Body = GecolCreateXml.CreditVendReqCreateXml.CreateSoapBody(MeterNumber, UniqeNumber, PurchaseValue);

                (string Responce, string StatusCode, Boolean state) SoapRsp = await soapServiceClient.SendSoapRequest(Body, SOAPAction.CreditVendReq.ToString());

                if (SoapRsp.state == true)
                {
                    string CreditToken;
                    try
                    {
                        CreditToken = await GecolConvertRsp.ConvCreditVendCRsp.Converte(SoapRsp.Responce);

                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        var InnerException = ex.InnerException;
                        string ExcLocation = ex.StackTrace.Replace("\n", "|");

                        CreditToken = "Exception";
                    }

                    return (CreditToken, SoapRsp.StatusCode, SoapRsp.state);


                }
                else
                {
                    return (await FailedCase(SoapRsp.Responce), SoapRsp.StatusCode, SoapRsp.state);

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");

                return ("Fault", message, false);
            }
        }



        public async static Task<string> FailedCase(string FailedXmlRespons)
        {
            try
            {
                xmlvendFaultRespFault faultModel = await GecolConvertRsp.ConvFaultRsp.Converte(FailedXmlRespons);

                return faultModel.FaultCode;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                var InnerException = ex.InnerException;
                string ExcLocation = ex.StackTrace.Replace("\n", "|");

                return ("Fault");
            }
        }


    }
}
