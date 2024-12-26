using GecolPro.Models.Gecol;
using GecolPro.Models.Models;

namespace GecolPro.BusinessRules.Interfaces
{
    //public interface IMenus
    //{
    //    public Task<MsgContent> SuccessResponseAsync(List<string> Arg, string Lang);
    //    //public Task<MsgContent> SuccessResponseAsync(SuccessResponseCreditVend Arg, string Lang);

    //    public Task<MsgContent> BlockedResponseAsync(string Lang);
    //    public  Task<MsgContent> BlockedResponseAsync(string FaultCode, string Lang);
    //    public  Task<MsgContent> UnderMaintenance_Gecol(string FaultCode, string Lang);
    //    public  Task<MsgContent> UnderMaintenance_Billing(string FaultCode, string Lang);
    //}



    public interface IMenusX
    {
        public Task<MsgContent> SuccessResponseAsync(SuccessResponseCreditVend Arg, string Lang);
        public Task<MsgContent> BlockedResponseAsync(string Lang);
        public Task<MsgContent> BlockedResponseAsync(string FaultCode, string Lang);
        public Task<MsgContent> UnderMaintenance_Gecol(string FaultCode, string Lang);
        public Task<MsgContent> UnderMaintenance_Billing(string FaultCode, string Lang);
        public Task<MsgContent> HistoryRecordsAsync(List<Models.DbEntity.Request> Args, string Lang);
    }
}
