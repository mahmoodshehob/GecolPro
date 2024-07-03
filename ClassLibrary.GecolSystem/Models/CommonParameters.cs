using Newtonsoft.Json;

namespace ClassLibrary.GecolSystem.Models
{
    public class CommonParameters : AuthCred
    {
        private static readonly Random Random = new ();
        public string UniqueNumber { get; set; }
        public string DateTimeReq { get; set; }


        public CommonParameters()
        {
            UniqueNumber = Random.Next(1, 999999).ToString("D6");
            DateTimeReq = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}