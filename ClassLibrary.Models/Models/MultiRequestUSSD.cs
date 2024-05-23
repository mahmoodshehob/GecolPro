using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ClassLibrary.Models.Models
{

    public class MultiRequestUSSD
    {

        public class MultiRequestSerXml
        {

            [XmlRoot(ElementName = "value")]
            public class Value
            {

                [XmlElement(ElementName = "string")]
                public string String { get; set; }

                [XmlElement(ElementName = "dateTime.iso8601")]
                public string DateTimeIso8601 { get; set; }
            }

            [XmlRoot(ElementName = "member")]
            public class Member
            {

                [XmlElement(ElementName = "name")]
                public string Name { get; set; }

                [XmlElement(ElementName = "value")]
                public Value Value { get; set; }
            }

            [XmlRoot(ElementName = "struct")]
            public class Struct
            {

                [XmlElement(ElementName = "member")]
                public List<Member> Member { get; set; }
            }

            [XmlRoot(ElementName = "values")]
            public class Values
            {

                [XmlElement(ElementName = "struct")]
                public Struct Struct { get; set; }
            }

            [XmlRoot(ElementName = "param")]
            public class Param
            {

                [XmlElement(ElementName = "value")]
                public Values Values { get; set; }
            }

            [XmlRoot(ElementName = "params")]
            public class Params
            {

                [XmlElement(ElementName = "param")]
                public Param Param { get; set; }
            }

            [XmlRoot(ElementName = "methodCall")]
            public class MethodCall
            {

                [XmlElement(ElementName = "methodName")]
                public string MethodName { get; set; }

                [XmlElement(ElementName = "params")]
                public Params Params { get; set; }
            }
        }

        public class MultiRequest
        {
            public string TransactionId { get; set; }
            public string TransactionTime { get; set; }
            public string MSISDN { get; set; }
            public string USSDServiceCode { get; set; }
            public string USSDRequestString { get; set; }
            public string Response { get; set; }
        }
    }
}
