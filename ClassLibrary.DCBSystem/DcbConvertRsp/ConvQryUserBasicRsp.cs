using ClassLibrary.DCBSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLibrary.DCBSystem.Models.QryUserBasicBalRspXml;
using System.Xml.Serialization;

namespace ClassLibrary.DCBSystem.DcbConvertRsp
{
    public class ConvQryUserBasicRsp
    {

        private static XmlSerializer serializer = new XmlSerializer(typeof(QryUserBasicBalRspXml.Envelope));
        
        private static BalanceDto balanceDto = new BalanceDto();

        private static string MSISDN;



        public static async Task<QryUserBasicBalRsp> Converte(string SoapRsp)
        {
            QryUserBasicBalRsp qryUserBasicBalRsp = new QryUserBasicBalRsp();

            using (StringReader reader = new StringReader(SoapRsp))
            {
                await Task.Run(() =>
                {
                    var RespQryUserBasicBal = (QryUserBasicBalRspXml.Envelope)serializer.Deserialize(reader);

                    //qryUserBasicBalReqx.MSISDN = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.MSISDN;

                    //MSISDN = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.MSISDN;
                    //balanceDto = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.BalanceDtoList.BalanceDto;

                    qryUserBasicBalRsp = new QryUserBasicBalRsp()
                    {
                        BalanceDto = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.BalanceDtoList.BalanceDto,
                        MSISDN = RespQryUserBasicBal.Body.QryUserBasicBalResponse.QryUserBasicBalResponseN.MSISDN
                    };
                }).ConfigureAwait(false);

                //return 
                //    new QryUserBasicBalRsp
                //    {
                //        BalanceDto = balanceDto,
                //        MSISDN = MSISDN
                //    };

                return qryUserBasicBalRsp;
            }
        }
    }
}