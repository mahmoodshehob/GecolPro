using GecolPro.Models.DbEntity;

namespace GecolPro.DataAccess.Interfaces
{
    public interface IRequestService
    {

        Task<ServiceResult> SaveGecolRequest(string? conversationId, string? MSISDN, string amount, bool status,
            string[] token, string uniqueNumber, string totalTax);
        Task<ServiceResult> SaveDcblRequest(string? conversationId, string? MSISDN, string amount, bool status, string transactionId);

        Task<List<Request>> GetAll();
    }
}
