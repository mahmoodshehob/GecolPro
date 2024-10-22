namespace GecolPro.Models.Gecol
{
    public class FaultModel
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public class Envelope
        {
            /// <remarks/>
            public object Header { get; set; }

            /// <remarks/>
            public EnvelopeBody Body { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public  class EnvelopeBody
        {
            /// <remarks/>
            public EnvelopeBodyFault Fault { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public  class EnvelopeBodyFault
        {

  
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public string faultcode { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public faultstring faultstring { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public detail detail { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
        public  class faultstring
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string lang { get; set; }


            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value { get; set; }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
        public  class detail
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public xmlvendFaultResp xmlvendFaultResp { get; set; }
        }



        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public  class xmlvendFaultResp
        {
            /// <remarks/>
            public xmlvendFaultRespFault Fault { get; set; }
        }


        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        public partial class xmlvendFaultRespFault
        {
            /// <remarks/>
            public string Desc { get; set; }

            /// <remarks/>
            public string FaultCode { get; set; }   
        }
    }
}
