using GecolPro.Models.Models;

namespace GecolPro.WebApi.Interfaces
{
    public interface IMenus
    {
        public Task<MsgContent> SuccessResponseAsync(List<string> Arg, string Lang);
        public  Task<MsgContent> BlockedResponseAsync(string Lang);
        public  Task<MsgContent> BlockedResponseAsync(string FaultCode, string Lang);
        public  Task<MsgContent> UnderMaintenance_Gecol(string FaultCode, string Lang);
        public  Task<MsgContent> UnderMaintenance_Billing(string FaultCode, string Lang);
    }
}
