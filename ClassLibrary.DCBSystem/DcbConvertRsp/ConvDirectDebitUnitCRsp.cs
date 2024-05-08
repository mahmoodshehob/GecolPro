using ClassLibrary.DCBSystem.Models;

using static ClassLibrary.DCBSystem.Models.DirectDebitUnitRspXml;

using System.Xml.Serialization;

namespace ClassLibrary.DCBSystem.DcbConvertRsp
{
    public class ConvDirectDebitUnitCRsp
    {

        private static XmlSerializer serializer = new XmlSerializer(typeof(DirectDebitUnitRspXml.Envelope));

        private static DirectDebitUnitRsp directDebitUnitRsp = new DirectDebitUnitRsp();



        public static async Task<DirectDebitUnitRsp> Converte(string SoapRsp)
        {
            using (StringReader reader = new StringReader(SoapRsp))
            {
                await Task.Run(() =>
                {
                    var RespDirectDebitUnit = (DirectDebitUnitRspXml.Envelope)serializer.Deserialize(reader);

                    directDebitUnitRsp = new DirectDebitUnitRsp()
                    {
                        ConversationID = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.ConversationID,
                        TransactionID = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.TransactionID,
                        Amount = RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp.Amount,
                    };

                    return RespDirectDebitUnit.Body.DirectDebitUnitResponse.DirectDebitUnitResp;


                }).ConfigureAwait(false);
            }

            return directDebitUnitRsp;
        }
    }  
}