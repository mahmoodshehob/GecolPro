using GecolPro.SmppClient.Models;

namespace GecolPro.SmppClient.Services.IServices
{
    public interface IServiceLogic
    {

        public Task<(bool, string?)> Post(SmsToKannel message);

        public Task<(bool, string?)> DLR(string phone, string msgid, string status, string deliveryDate);
        
        public Task<(bool, string?)> KannelStatus();
    }
}
