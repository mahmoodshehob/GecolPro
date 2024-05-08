using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.DcbModels
{
    public class AuthHeader
    {
        private string _username = "SMSC_NS_DCB";
        private string _password = "S3IS9C8D_HI6P4S5";
        private string _url = "http://192.168.120.25:8223/ocswebservices/services/WebServicesSoapLibya";


        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
    }
}