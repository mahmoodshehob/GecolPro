using ClassLibrary.GecolSystem.Models;
using System.Xml;
using System.Xml.Serialization;
using static ClassLibrary.GecolSystem.Models.FaultModel;




namespace ClassLibrary.GecolSystem.GecolConvertRsp
{
    public class ConvFaultRsp
    {

        private static XmlSerializer serializer = new XmlSerializer(typeof(FaultModel.Envelope));

        private static xmlvendFaultRespFault respFault = new xmlvendFaultRespFault();


        //public static async Task<xmlvendFaultRespFault> Converte(string SoapRsp)
        //{

        //    gggggggg(SoapRsp);

        //    using (StringReader reader = new StringReader(SoapRsp))
        //    {
        //        await Task.Run(() =>
        //        {
        //            var Envelope = (FaultModel.Envelope)serializer.Deserialize(reader);

        //            respFault = Envelope.Body.Fault.detail.xmlvendFaultResp.Fault;

        //            return respFault;

        //        }).ConfigureAwait(false);
        //    }

        //    return respFault;
        //}





        public static async Task<xmlvendFaultRespFault> Converte(string SoapRsp)
        {
            

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(SoapRsp); // Load the XML from the string

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns2", "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema");

            XmlElement root = doc.DocumentElement;
            try
            {
                if (root != null)
                {
                    XmlNodeList list = root.SelectNodes("//ns2:fault", nsmgr); // Use XPath to find the right nodes with namespace
                    foreach (XmlNode node in list)
                    {
                        string FaultCode = node["ns2:faultCode"].InnerText;

                        string FaultDescription = node["ns2:desc"].InnerText;


                        respFault = new xmlvendFaultRespFault()
                        {
                            FaultCode = FaultCode,
                            Desc = FaultDescription
                        };
                        return respFault;
                    }
                }
                return respFault;
            }catch (Exception ex)
            {
                return new xmlvendFaultRespFault()
                {
                    FaultCode = ex.Message,
                    Desc = ex.Message
                };
            }
        }


        //private static async void gggggggg(string xmlContent)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent); // Load the XML from the string

        //    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        //    nsmgr.AddNamespace("ns2", "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema");

        //    XmlElement root = doc.DocumentElement;
        //    if (root != null)
        //    {
        //        XmlNodeList list = root.SelectNodes("//ns2:fault", nsmgr); // Use XPath to find the right nodes with namespace
        //        foreach (XmlNode node in list)
        //        {
        //            string FaultDescription =  node["ns2:desc"].InnerText;
        //            string FaultCode = node["ns2:faultCode"].InnerText;
        //        }
        //    }
        //}


    }
}