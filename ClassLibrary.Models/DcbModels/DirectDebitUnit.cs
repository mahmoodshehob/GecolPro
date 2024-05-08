using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLibrary.Models.DcbModels.DirectDebitUnitRspXml;

namespace ClassLibrary.Models.DcbModels
{
    public class DirectDebitUnitReqSoap
    {
        public string ConversationID { get; set; }
        public string TransactionID { get; set; }
        public string ServiceName { get; set; }
        public string ProviderName { get; set; }
        public string OriginatingAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string ChargingAddress { get; set; }
        public int Amount { get; set; }
    }



    public class DirectDebitUnitRspXml
    {


        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            /// <remarks/>
            public EnvelopeBody Body { get; set; }

        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
            public DirectDebitUnitResponse DirectDebitUnitResponse { get; set; }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com", IsNullable = false)]
        public partial class DirectDebitUnitResponse
        {
            public DirectDebitUnitResp DirectDebitUnitResp { get; set; }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        public partial class DirectDebitUnitResp
        {
            public string ConversationID { get; set; }
            public string TransactionID { get; set; }
            public string Amount { get; set; }
        }
    }

    public class DirectDebitUnitRsp
    {
        public string MSISDN { get; set; }
        public string ProviderName { get; set; }
        public DirectDebitUnitResp DirectDebitUnitResp { get; set; }
    }
}