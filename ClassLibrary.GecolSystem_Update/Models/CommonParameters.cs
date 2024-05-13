using Newtonsoft.Json;

namespace ClassLibrary.GecolSystem_Update.Models
{
    public class CommonParameters : AuthCred
    {
        private static readonly Random Random = new ();

        public string EANDeviceID { get; set; }
        public string GenericDeviceID { get; set; }

        public string UniqueNumber { get; set; }

        public string DateTimeReq { get; set; }


        public CommonParameters()
        {
            var json = File.ReadAllText("defaultValues.json");
            var defaultValues = JsonConvert.DeserializeObject<DefaultValues>(json);
            EANDeviceID = defaultValues!.EanDeviceId!;
            GenericDeviceID = defaultValues.GenericDeviceId!;
            UniqueNumber = Random.Next(1, 999999).ToString("D6");
            DateTimeReq = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}