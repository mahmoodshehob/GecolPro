using ClassLibrary.DCBSystem.Models;
using ClassLibrary.Models.Models;
using ClassLibrary.Services;


namespace ClassLibrary.DCBSystem
{
    public class DcbOperation
    {
        private static SoapServiceClient soapServiceClient = new SoapServiceClient();

        private static QryUserBasicBalRsp qryUserBasicBalRsp = new QryUserBasicBalRsp();

        private static DirectDebitUnitRsp directDebitUnitRsp = new DirectDebitUnitRsp();

        private static Loggers LoggerG = new Loggers();

        //private static string FaultString = "";

        private enum SOAPAction
        {
            QryUserBasicBal,
            DirectDebitUnit,
            DebitRollback
        }


        public async static Task<(string Response, string StatusCode, Boolean Status)> QryUserBasicBalOp(string Msisdn)
        {
            QryUserBasicBalSoap qryUserBasicBalSoap = new QryUserBasicBalSoap()
            {
                MSISDN = Msisdn,
            };

            qryUserBasicBalRsp.MSISDN = Msisdn;
                 
            string Body = DcbCreateXml.QryUserBalCreateXml.CreateSoapBody(qryUserBasicBalSoap);

            (string Responce, string StatusCode, Boolean Status) SoapRsp = await soapServiceClient.SendSoapRequest(Body, SOAPAction.QryUserBasicBal.ToString());



            if (SoapRsp.Status == true)
            {
                qryUserBasicBalRsp = await DcbConvertRsp.ConvQryUserBasicRsp.Converte(SoapRsp.Responce);
      
                string Balance = (int.Parse(qryUserBasicBalRsp.BalanceDto.BalanceValue) / 100000).ToString();

                return (Balance , SoapRsp.StatusCode, true);
            }
            else
            {
                string FaultCode;
                if (SoapRsp.Responce.ToLower() != "timeout")
                {
                    FaultCode = await FailedCase(SoapRsp.Responce);
                }
                else
                {
                    FaultCode = "timeout";
                }

                return (FaultCode, SoapRsp.StatusCode, false);
            }
        }



        public async static Task<(string Response, string StatusCode, Boolean Status)> DirectDebitUnitOp(string ConversationID, string Msisdn , int Amount)
        {

            DirectDebitUnitReqSoap directDebitUnitReq =new DirectDebitUnitReqSoap()
            {
                ServiceName = "Gecol",

                ConversationID = ConversationID,
                DestinationAddress = Msisdn ,
                Amount = Amount * 1000
            };
            
            string Body = DcbCreateXml.DirectDebitUnitCreateXml.CreateSoapBody(directDebitUnitReq);




            (string Responce, string StatusCode, Boolean Status) SoapRsp = await soapServiceClient.SendSoapRequest(Body, SOAPAction.DirectDebitUnit.ToString());








            if (SoapRsp.Status == true)
            {
                directDebitUnitRsp = await DcbConvertRsp.ConvDirectDebitUnitCRsp.Converte(SoapRsp.Responce);

                return  (directDebitUnitRsp.TransactionID, SoapRsp.StatusCode, SoapRsp.Status);
            }
            else
            {
                string FaultCode;
                if (SoapRsp.Responce.ToLower() != "timeout")
                {
                    
                     FaultCode  = await FailedCase(SoapRsp.Responce);

                }
                else
                {

                     FaultCode = "timeout";

                }
                return (FaultCode, SoapRsp.StatusCode, SoapRsp.Status);
            }
        }



        public async static Task<string> FailedCase(string FailedXmlRespons)
        {
            try
            {
                FaultModel faultModel = await DcbConvertRsp.ConvFaultRsp.Converte(FailedXmlRespons);

                await LoggerG.LogErrorAsync($"FaultCode : {faultModel.FaultCode} |FaultString : {faultModel.FaultString}");


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
