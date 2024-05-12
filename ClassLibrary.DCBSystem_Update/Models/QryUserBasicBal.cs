namespace ClassLibrary.DCBSystem_Update.Models
{
    public class QryUserBasicBalSoap
    {
        public string MSISDN { get; set; }
    }



    public class QryUserBasicBalRspXml
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
            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]

            public QryUserBasicBalResponse QryUserBasicBalResponse { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com", IsNullable = false)]
        public partial class QryUserBasicBalResponse
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElement("QryUserBasicBalResponse")]

            public QryUserBasicBalResponseN QryUserBasicBalResponseN { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        public partial class QryUserBasicBalResponseN
        {
            /// <remarks/>
            //public ulong MSISDN { get; set; }
            public string MSISDN { get; set; }

            /// <remarks/>
            public BalanceDtoList BalanceDtoList { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        public partial class BalanceDtoList
        {
            /// <remarks/>
            public BalanceDto BalanceDto { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://libya.customization.ws.bss.zsmart.ztesoft.com")]
        public partial class BalanceDto
        {
            public string? BalanceID { get; set; }
            public string? BalanceName { get; set; }
            public string? BalanceValue { get; set; }
            public string? UnitType { get; set; }
            public string? EffDate { get; set; }
            public string? ExpDate { get; set; }
            public string? Comments { get; set; }
        }
    }

    public class QryUserBasicBalRsp 
    {
        public string MSISDN { get; set; }
        //public QryUserBasicBalRspXml.BalanceDto BalanceDto { get; set; }
        public string? CommentsOrBalance { get; set; }

        public QryUserBasicBalRspXml.BalanceDto BalanceDto { get; set; }
    }
}
