namespace ClassLibrary.GecolSystem_Update.Models
{
    public class CommonParameters : AuthCred
    {
        private static readonly Random Random = new ();

        public string EANDeviceID { get; set; }
        public string GenericDeviceID { get; set; }

        public string UniqueNumber { get; set; }

        public string DateTimeReq { get; set; }


        public CommonParameters(string eanDeviceId = "0000000000001", string genericDeviceId = "0000000000001")
        {
            EANDeviceID = eanDeviceId;
            GenericDeviceID = genericDeviceId;
            UniqueNumber = Random.Next(1, 999999).ToString("D6");
            DateTimeReq = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}