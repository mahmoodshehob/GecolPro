using GecolPro.Models.DbEntity;

namespace GecolPro.DataAccess.Interfaces
{
    public interface IRequestService
    {

        Task<ServiceResult> SaveGecolRequest(string? conversationId, string? MSISDN, string meterNumber, string amount, bool status, string transactionId,string[] token, string uniqueNumber, string totalTax);
        Task<ServiceResult> SaveDcblRequest(string? conversationId, string? MSISDN, string meterNumber, string amount, bool status, string transactionId);

        Task<List<Request>> GetAll();

        Task<List<Request>> QueryTokenHistoryAll(string Msisdn, int previous = 30);
    }
}
