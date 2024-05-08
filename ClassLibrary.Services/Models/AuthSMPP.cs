using ClassLibrary.Services.IniFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace ClassLibrary.Services.Models
{
    public class AuthSMPP
    {

        private string _ipAddress   = "localhost";
        private int    _port        = 13013;
        private string _username    = "kannel";
        private string _password    = "kannel";




        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public string Port
        {
            get { return _port.ToString(); }
            set { _port = int.Parse(value); }
        }


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
    }
}

