using ClassLibrary.DataAccess.Models;

namespace ClassLibrary.DataAccess.Interfaces
{
    public interface IRequestService
    {
    
        Task<ServiceResult> SaveGecolRequest(string? conversationId, string? MSISDN,string amount, bool status,string token,string uniqueNumber);
        Task<ServiceResult> SaveDcblRequest(string? conversationId, string? MSISDN, string amount, bool status,string transactionId);

        Task<List<Request>> GetAll();
    }
}
