using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassLibrary.DCBSystem.Models;

namespace ClassLibrary.DCBSystem.DcbConvertRsp
{
    public class ConvFaultRsp
    {
        private static XNamespace soapen = "http://schemas.xmlsoap.org/soap/envelope/";

        private static XNamespace ns = "http://libya.customization.ws.bss.zsmart.ztesoft.com";

        private static XNamespace ns1 = "http://xml.apache.org/axis/";

        public  static async Task<FaultModel> Converte(string xmlString)
        {


            FaultModel reqElement = new FaultModel();


            XDocument doc = XDocument.Parse(xmlString);

            var ResponceObject = doc
                .Element(soapen + "Envelope")
                .Element(soapen + "Body")
                .Descendants(soapen + "Fault");

            reqElement.FaultCode = ResponceObject.Descendants("faultcode").FirstOrDefault().Value;
            reqElement.FaultString = ResponceObject.Descendants("faultstring").FirstOrDefault().Value;
            reqElement.Detail = ResponceObject.Elements("detail").Descendants(ns1 + "hostname").FirstOrDefault().Value;

            return reqElement;
        }
    }
}