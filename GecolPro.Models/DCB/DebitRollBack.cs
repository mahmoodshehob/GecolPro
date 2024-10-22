using static GecolPro.Models.DCB.DebitObjModels;

namespace GecolPro.Models.DCB
{
    public class DebitRollbackReqSoap : DebitObjReq
    {
    }

    public class DebitRollBackRsp : DebitRollbackRspXml.DebitRollbackResp
    {
    }

    public class DebitRollbackRspXml
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            /// <remarks/>
            public EnvelopeBody Body { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            [System.Xml.Serialization.XmlElement(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
            public DebitRollbackResponse DebitRollbackResponse { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com", IsNullable = false)]
        public partial class DebitRollbackResponse
        {
            public DebitRollbackResp DebitRollbackResp { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        public partial class DebitRollbackResp
        {
            public string ConversationID { get; set; }
            public string TransactionID { get; set; }
            public string Amount { get; set; }
        }
    }
}