using static ClassLibrary.Models.UssdModels.MultiResponseUSSD;

namespace GecolPro.Main.Process
{
    public class Validation
    {
        private static string MPage = "Page1";


        private static readonly Dictionary<string, string> serviceName = new Dictionary<string, string>()
        {
            { MPage, "MainService" },
            { MPage+"1", "MeterID" },
            { MPage+"11", "Balance" }
        };

    }
}
