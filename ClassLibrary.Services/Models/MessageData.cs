using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Services.Models
{
    public class MessageData
    {
        private string _sendername = "2188997772";
        private string _receiver;
        private string _message;



        public string? Sender
        {
            get { return this._sendername; }
            set { this._sendername = value; }
        }

        public string Receiver
        {
            get { return this._receiver; }
            set { this._receiver = value; }
        }

        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
    }

   
}
