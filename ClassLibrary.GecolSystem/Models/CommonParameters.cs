using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.GecolSystem.Models
{
    //public class CommonParameters : AuthCred
    //{
    //    private static readonly Random _random = new Random();
    //    private string _uniqueNumber = _random.Next(1, 999999).ToString("D6");
    //    private string _dateTimeReq = DateTime.Now.ToString("yyyyMMddHHmmss");



    //    public string EANDeviceID
    //    {
    //        get { return "0000000000002"; }
    //    }

    //    public string GenericDeviceID
    //    {
    //        get { return "0000000000002"; }
    //    }
    //    public string DateTimeReq
    //    {
    //        get { return _dateTimeReq; }
    //        set { _dateTimeReq = value; }
    //    }

    //    public string UniqueNumber
    //    {
    //        get { return _uniqueNumber; }
    //        set { _uniqueNumber = value; }
    //    }

    //}


    public class CommonParameters : AuthCred
    {
        private static readonly Random _random = new();


        public string UniqueNumber { get; set; }
        public string DateTimeReq { get; set; }


        public CommonParameters()
        {
            UniqueNumber = _random.Next(1, 999999).ToString("D6");
            DateTimeReq = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}