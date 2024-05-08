//using ClassLibrary.DCBSystem.Models;

//using System.Xml.Serialization;
//using static ClassLibrary.DCBSystem.Models.DebitRollbackRspXml;

//namespace ClassLibrary.DCBSystem.DcbConvertRsp
//{
//    public class ConvDebitRollBackCRsp
//    {

//        private static XmlSerializer serializer = new XmlSerializer(typeof(DebitRollbackRspXml.Envelope));

//        private static DebitRollBackRsp debitRollbackRsp = new DebitRollBackRsp();


//        public static async Task<DebitRollBackRsp> Converte(string SoapRsp)
//        {
//            using (StringReader reader = new StringReader(SoapRsp))
//            {
//                await Task.Run(() =>
//                {
//                    var RespDebitRollback = (DebitRollbackRspXml.Envelope)serializer.Deserialize(reader);

//                    //return new DirectDebitUnitRsp()
//                    //{
//                    //    ConversationID = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.ConversationID,
//                    //    TransactionID = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.TransactionID,
//                    //    Amount = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.Amount
//                    //};

//                    return RespDebitRollback.Body.DebitRollbackResponse.DebitRollbackResp;

//                }).ConfigureAwait(false);
//            }

//            return debitRollbackRsp;
//        }
//    }  
//}