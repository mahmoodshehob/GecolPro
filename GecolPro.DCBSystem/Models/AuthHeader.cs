using Newtonsoft.Json;

namespace GecolPro.DCBSystem.Models
{
    public class AuthHeader
    {
        public AuthHeader()
        {

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "defaultValues_DCB.json");
            var json = File.ReadAllText(jsonFilePath);
            var defaultValues = JsonConvert.DeserializeObject<DefaultValues>(json);



            Username = defaultValues!.Username;
            Password = defaultValues.Password;
            Url = new Uri(defaultValues.Url!);
        }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public Uri Url { get; set; }
    }
}