using ClassLibrary.DataAccess.Interfaces;
using ClassLibrary.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DataAccess.Services
{
    public class RequestService: IRequestService
    {
        private readonly AppDbContext _context;

        public RequestService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> AddRequest(Request request)
        {
             _context.Requests.Add(request);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<ServiceResult> SaveGecolRequest(string? conversationId, string? MSISDN, string amount, bool status, string token, string uniqueNumber)
        {
            try
            {
                Request request = new()
                {
                    ConversationId = conversationId,
                    MSISDN = MSISDN,
                    Status = status,
                    Token = token,
                    Amount = amount,
                    TransactionId = "",
                    UniqueNumber = uniqueNumber,
                    FromSystem = "Gecol"

                };

                var result = await AddRequest(request);

                return new ServiceResult(result, result ? "The saving process was successful." : "Unknown error");
            }
            catch (Exception e)
            {
                return new ServiceResult(false, e.Message);
            }

        }

        public async Task<ServiceResult> SaveDcblRequest(string? conversationId, string? MSISDN, string amount, bool status, string transactionId)
        {
            try
            {
                Request request = new()
                {
                    ConversationId = conversationId,
                    MSISDN = MSISDN,
                    Status = status,
                    Amount = amount,
                    Token = "",
                    TransactionId = transactionId,
                    UniqueNumber = "",
                    FromSystem = "Dcb"

                };

                var result = await AddRequest(request);

                return new ServiceResult(result, result ? "The saving process was successful." : "Unknown error");
            }
            catch (Exception e)
            {
                return new ServiceResult(false, e.Message);
            }
        }

        public async Task<List<Request>> GetAll()
        {
            return await _context.Requests.ToListAsync();
        }
    }
}
