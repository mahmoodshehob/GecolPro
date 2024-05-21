using Newtonsoft.Json;


namespace ClassLibrary.GecolSystem.Models
{
    public class AuthCred
    {
        public AuthCred()
        {

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "defaultValues_Gecol.json");
            var json = File.ReadAllText(jsonFilePath);
            var defaultValues = JsonConvert.DeserializeObject<DefaultValues>(json);

            Username = defaultValues!.Username;
            Password = defaultValues.Password;
            Url = defaultValues.Url!;
            EANDeviceID = defaultValues.EanDeviceId!;
            GenericDeviceID = defaultValues.GenericDeviceId!;
        }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Url { get; set; }
        public string EANDeviceID { get; set; }
        public string GenericDeviceID { get; set; }
    }
}