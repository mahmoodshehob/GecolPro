using GecolPro.Models.Models;


namespace GecolPro.BusinessRules.Interfaces
{
    public class IUssdService
    {
    }


    public interface IUssdConverter
    {
        public Task<MultiRequestUSSD.MultiRequest> ConverterFaster(string xmlString);

    }


    public interface IResponses
    {
        public string Resp(MultiResponseUSSD multiResponse);
        public string Fault_Response(MultiResponseUSSD multiResponse);
    }
}
