using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLibrary.GecolSystem.Models.LoginRspXml;

namespace ClassLibrary.GecolSystem.Models
{
    public class LoginReq : CommonParameters
    {

    }


    public class LoginRspXml
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            public object Header { get; set; }

            public EnvelopeBody Body { get; set; }

        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
            //public user user { get; set; }
            public LoginRsp user { get; set; }

        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema", IsNullable = false)]
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