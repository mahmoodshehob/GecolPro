using GecolPro.SmppClient.Models;

namespace GecolPro.SmppClient.Services.IServices
{
    public interface IServiceLogic
    {

        public Task<ResulteModel> Post(SmsToKannel message);

        public Task<ResulteModel> DLR(string phone, string msgid, string status, string deliveryDate);
        
        public Task<ResulteModel> KannelStatus();
    }
}
