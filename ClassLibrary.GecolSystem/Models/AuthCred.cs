using System;

namespace ClassLibrary.GecolSystem.Models
{
    public class AuthCred
    {
        private string _username = "AG0502";
        private string _password = "1234567891012";
        private string _url = "http://160.19.103.138:40808/xmlvend/xmlvend.wsdl";


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