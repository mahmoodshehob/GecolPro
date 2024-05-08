//using ClassLibrary.GecolSystem.Models;

//using System.Xml.Serialization;
//using static ClassLibrary.GecolSystem.Models.ConfirmCustomerRspXml;

//namespace ClassLibrary.GecolSystem.GecolConvertRsp
//{
//    public class ConvConfirmCustomerRsp
//    {

//        private static XmlSerializer serializer = new XmlSerializer(typeof(ConfirmCustomerRspXml.Envelope));

//        private static LoginRsp loginRsp = new LoginRsp();


//        public static async Task<LoginRsp> Converte(string SoapRsp)
//        {
//            using (StringReader reader = new StringReader(SoapRsp))
//            {
//                await Task.Run(() =>
//                {
//                    var Envelope = (LoginRspXml.Envelope)serializer.Deserialize(reader);

//                    loginRsp = Envelope.Body.user;
                    
//                    return loginRsp;

//                }).ConfigureAwait(false);
//            }

//            return loginRsp;
//        }
//    }  
//}