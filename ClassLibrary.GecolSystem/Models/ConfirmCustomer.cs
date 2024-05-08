
using System.ComponentModel.DataAnnotations;


namespace ClassLibrary.GecolSystem.Models
{
    public class ConfirmCustomerReq : CommonParameters
    {
        [RegularExpression(@"^\d{13}$", ErrorMessage = "The MeterNumber must be a 12-digit number.")]
        public string MeterNumber { set; get; }
    }

    public class ConfirmCustomerRspXml
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private object headerField;

            private EnvelopeBody bodyField;

            /// <remarks/>
            public object Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private confirmCustomerResp confirmCustomerRespField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
            public confirmCustomerResp confirmCustomerResp
            {
                get
                {
                    return this.confirmCustomerRespField;
                }
                set
                {
                    this.confirmCustomerRespField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema", IsNullable = false)]
        public partial class confirmCustomerResp
        {

            private clientID clientIDField;

            private serverID serverIDField;

            private terminalID terminalIDField;

            private reqMsgID reqMsgIDField;

            private System.DateTime respDateTimeField;

            private object dispHeaderField;

            private confirmCustomerRespConfirmCustResult confirmCustResultField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public clientID clientID
            {
                get
                {
                    return this.clientIDField;
                }
                set
                {
                    this.clientIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public serverID serverID
            {
                get
                {
                    return this.serverIDField;
                }
                set
                {
                    this.serverIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public terminalID terminalID
            {
                get
                {
                    return this.terminalIDField;
                }
                set
                {
                    this.terminalIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public reqMsgID reqMsgID
            {
                get
                {
                    return this.reqMsgIDField;
                }
                set
                {
                    this.reqMsgIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public System.DateTime respDateTime
            {
                get
                {
                    return this.respDateTimeField;
                }
                set
                {
                    this.respDateTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public object dispHeader
            {
                get
                {
                    return this.dispHeaderField;
                }
                set
                {
                    this.dispHeaderField = value;
                }
            }

            /// <remarks/>
            public confirmCustomerRespConfirmCustResult confirmCustResult
            {
                get
                {
                    return this.confirmCustResultField;
                }
                set
                {
                    this.confirmCustResultField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class clientID
        {

            private byte eanField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte ean
            {
                get
                {
                    return this.eanField;
                }
                set
                {
                    this.eanField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class serverID
        {

            private ulong eanField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong ean
            {
                get
                {
                    return this.eanField;
                }
                set
                {
                    this.eanField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class terminalID
        {

            private byte idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class reqMsgID
        {

            private ulong dateTimeField;

            private byte uniqueNumberField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong dateTime
            {
                get
                {
                    return this.dateTimeField;
                }
                set
                {
                    this.dateTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte uniqueNumber
            {
                get
                {
                    return this.uniqueNumberField;
                }
                set
                {
                    this.uniqueNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResult
        {

            private confirmCustomerRespConfirmCustResultCustVendDetail custVendDetailField;

            private confirmCustomerRespConfirmCustResultCustVendConfig custVendConfigField;

            private confirmCustomerRespConfirmCustResultMeterDetail meterDetailField;

            /// <remarks/>
            public confirmCustomerRespConfirmCustResultCustVendDetail custVendDetail
            {
                get
                {
                    return this.custVendDetailField;
                }
                set
                {
                    this.custVendDetailField = value;
                }
            }

            /// <remarks/>
            public confirmCustomerRespConfirmCustResultCustVendConfig custVendConfig
            {
                get
                {
                    return this.custVendConfigField;
                }
                set
                {
                    this.custVendConfigField = value;
                }
            }

            /// <remarks/>
            public confirmCustomerRespConfirmCustResultMeterDetail meterDetail
            {
                get
                {
                    return this.meterDetailField;
                }
                set
                {
                    this.meterDetailField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResultCustVendDetail
        {

            private ulong accNoField;

            private string addressField;

            private byte contactNoField;

            private byte daysLastPurchaseField;

            private string locRefField;

            private ulong nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong accNo
            {
                get
                {
                    return this.accNoField;
                }
                set
                {
                    this.accNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte contactNo
            {
                get
                {
                    return this.contactNoField;
                }
                set
                {
                    this.contactNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte daysLastPurchase
            {
                get
                {
                    return this.daysLastPurchaseField;
                }
                set
                {
                    this.daysLastPurchaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string locRef
            {
                get
                {
                    return this.locRefField;
                }
                set
                {
                    this.locRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResultCustVendConfig
        {

            private bool canVendField;

            private bool fbeDueField;

            private confirmCustomerRespConfirmCustResultCustVendConfigMaxVendAmt maxVendAmtField;

            private confirmCustomerRespConfirmCustResultCustVendConfigMinVendAmt minVendAmtField;

            /// <remarks/>
            public bool canVend
            {
                get
                {
                    return this.canVendField;
                }
                set
                {
                    this.canVendField = value;
                }
            }

            /// <remarks/>
            public bool fbeDue
            {
                get
                {
                    return this.fbeDueField;
                }
                set
                {
                    this.fbeDueField = value;
                }
            }

            /// <remarks/>
            public confirmCustomerRespConfirmCustResultCustVendConfigMaxVendAmt maxVendAmt
            {
                get
                {
                    return this.maxVendAmtField;
                }
                set
                {
                    this.maxVendAmtField = value;
                }
            }

            /// <remarks/>
            public confirmCustomerRespConfirmCustResultCustVendConfigMinVendAmt minVendAmt
            {
                get
                {
                    return this.minVendAmtField;
                }
                set
                {
                    this.minVendAmtField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResultCustVendConfigMaxVendAmt
        {

            private string symbolField;

            private uint valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string symbol
            {
                get
                {
                    return this.symbolField;
                }
                set
                {
                    this.symbolField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResultCustVendConfigMinVendAmt
        {

            private string symbolField;

            private byte valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string symbol
            {
                get
                {
                    return this.symbolField;
                }
                set
                {
                    this.symbolField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema")]
        public partial class confirmCustomerRespConfirmCustResultMeterDetail
        {

            private meterType meterTypeField;

            private byte maxVendAmtField;

            private decimal minVendAmtField;

            private string maxVendEngField;

            private string minVendEngField;

            private byte krnField;

            private ulong msnoField;

            private uint sgcField;

            private byte tiField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public meterType meterType
            {
                get
                {
                    return this.meterTypeField;
                }
                set
                {
                    this.meterTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public byte maxVendAmt
            {
                get
                {
                    return this.maxVendAmtField;
                }
                set
                {
                    this.maxVendAmtField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public decimal minVendAmt
            {
                get
                {
                    return this.minVendAmtField;
                }
                set
                {
                    this.minVendAmtField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public string maxVendEng
            {
                get
                {
                    return this.maxVendEngField;
                }
                set
                {
                    this.maxVendEngField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
            public string minVendEng
            {
                get
                {
                    return this.minVendEngField;
                }
                set
                {
                    this.minVendEngField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte krn
            {
                get
                {
                    return this.krnField;
                }
                set
                {
                    this.krnField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong msno
            {
                get
                {
                    return this.msnoField;
                }
                set
                {
                    this.msnoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint sgc
            {
                get
                {
                    return this.sgcField;
                }
                set
                {
                    this.sgcField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte ti
            {
                get
                {
                    return this.tiField;
                }
                set
                {
                    this.tiField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema", IsNullable = false)]
        public partial class meterType
        {

            private byte atField;

            private byte ttField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte at
            {
                get
                {
                    return this.atField;
                }
                set
                {
                    this.atField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte tt
            {
                get
                {
                    return this.ttField;
                }
                set
                {
                    this.ttField = value;
                }
            }
        }
    }

    public class ConfirmCustomerRsp
    {

    }
}
