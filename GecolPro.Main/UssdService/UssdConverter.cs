﻿using ClassLibrary.Models.Models;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using static ClassLibrary.Models.Models.MultiRequestUSSD.MultiRequestSerXml;
using static ClassLibrary.Models.Models.MultiRequestUSSD.MultiRequest;


namespace GecolPro.Main.UssdService

{
    public class UssdConverter
    {
        private static MultiRequestUSSD.MultiRequest multiRequestRE = new MultiRequestUSSD.MultiRequest();
        private static XmlSerializer serializer = new XmlSerializer(typeof(MethodCall));

        public static async Task<MultiRequestUSSD.MultiRequest> ConverterFaster(string xmlString)
        {
            using (StringReader reader = new StringReader(xmlString))
            {
                await Task.Run(() =>
                {
                    var methodCallReq = (MethodCall)serializer.Deserialize(reader);

                    Struct @struct = methodCallReq.Params.Param.Values.Struct;

                    foreach (var member in @struct.Member)
                    {
                        switch (member.Name)
                        {
                            case "TransactionId":
                                multiRequestRE.TransactionId = member.Value.String;
                                break;

                            case "TransactionTime":
                                if (member.Value.DateTimeIso8601 != null)
                                {
                                    multiRequestRE.TransactionTime = DateTime.ParseExact(member.Value.DateTimeIso8601, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture).ToString(); ;
                                }
                                else
                                {
                                    multiRequestRE.TransactionTime = DateTime.ParseExact(member.Value.String, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture).ToString(); ;
                                }
                                break;

                            case "MSISDN":
                                multiRequestRE.MSISDN = member.Value.String;
                                break;

                            case "USSDServiceCode":
                                multiRequestRE.USSDServiceCode = member.Value.String;
                                break;

                            case "USSDRequestString":
                                multiRequestRE.USSDRequestString = member.Value.String;
                                break;

                            case "response":
                                multiRequestRE.Response = member.Value.String;
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