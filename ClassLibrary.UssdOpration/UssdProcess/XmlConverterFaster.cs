using ClassLibrary.Models.ModelOfUssd;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static ClassLibrary.Models.ModelOfUssd.MultiRequestXmlSer;

namespace ClassLibrary.UssdOpration.UssdProcess
{
    public class XmlConverterFaster
    {
        private static MultiRequest multiRequestRE = new MultiRequest();
        private static XmlSerializer serializer = new XmlSerializer(typeof(MethodCall));

        public static async Task<MultiRequest> ConverterFaster(string xmlString)
        {   
            using (StringReader reader = new StringReader(xmlString))
            {
                await Task.Run(() => {
                    var methodCallReq = (MethodCall)serializer.Deserialize(reader);

                    Struct @struct = methodCallReq.Params.Param.Values.Struct;
                    
                    foreach (var member in @struct.Member)
                    {
                        switch (member.Name)
                        {
                            case "TransactionId":    multiRequestRE.TransactionId = member.Value.String;  
                                break;

                            case "TransactionTime":  multiRequestRE.TransactionTime = DateTime.ParseExact(member.Value.DateTimeIso8601, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture).ToString(); ;
                                break;

                            case "MSISDN":           multiRequestRE.MSISDN = member.Value.String; 
                                break;

                            case "USSDServiceCode":  multiRequestRE.USSDServiceCode = member.Value.String; 
                                break;

                            case "USSDRequestString":multiRequestRE.USSDRequestString = member.Value.String; 
                                break;

                            case "response":         multiRequestRE.response = member.Value.String; 
                                break;
                        }
                    }
                }).ConfigureAwait(false);
                return multiRequestRE;
            }
        }
    }
}


//   await Task.Run(() => { action(item); }).ConfigureAwait(false);