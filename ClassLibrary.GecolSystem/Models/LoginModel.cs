namespace ClassLibrary.GecolSystem.Models
{

    public class LoginRspXml
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            public object Header { get; set; }

            public EnvelopeBody Body { get; set; }

        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            [System.Xml.Serialization.XmlElement(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
            //public user user { get; set; }
            public LoginRsp user { get; set; }

        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema", IsNullable = false)]
        //public partial class user
        public partial class LoginRsp

        {

            public ushort ID { get; set; }

            public ushort TID { get; set; }

            public ushort CDUID { get; set; }

            public string CDUName { get; set; }

            public decimal CDUBalance { get; set; }

            public byte MinVendAmt { get; set; }

            public uint MaxVendAmt { get; set; }

            public System.DateTime LoginTime { get; set; }

        }
    }

    //public class LoginRsp : user
    //{
        
    //}

}