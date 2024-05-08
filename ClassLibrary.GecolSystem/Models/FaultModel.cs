using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.GecolSystem.Models
{
    public class FaultModel
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
            public object Header { get; set; }

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
            public EnvelopeBodyFault Fault { get; set; }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBodyFault
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
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class faultstring
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string lang { get; set; }


            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value { get; set; }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class detail
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public xmlvendFaultResp xmlvendFaultResp { get; set; }
        }



        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class xmlvendFaultResp
        {
            /// <remarks/>
            public xmlvendFaultRespFault Fault { get; set; }
        }


        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        public partial class xmlvendFaultRespFault
        {
            /// <remarks/>
            public string Desc { get; set; }

            /// <remarks/>
            public string FaultCode { get; set; }   
        }
    }
}
