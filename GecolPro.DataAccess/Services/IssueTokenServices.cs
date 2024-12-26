using GecolPro.DataAccess.Interfaces;
using GecolPro.Models.DbEntity;
using Microsoft.EntityFrameworkCore;

namespace GecolPro.DataAccess.Services
{
    public class IssueTokenServices : IIssueTokenServices
    {

        private readonly AppDbContext _context;

        public IssueTokenServices(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> AddIssueToken(IssueTkn token)
        {
            try
            {
                _context.IssueTkns?.Add(token);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {

                return false;
            }


        }

        public async Task<ServiceResult> CreateNew(string? conversationId, string? msisdn, string? dateTimeReq, string? uniqueNumber, string? meterNumber, int amount)
        {
            try
            {

                if (string.IsNullOrEmpty(conversationId))
                {
                    return new ServiceResult(false, "ConversationId  is Required");
                }
                if (string.IsNullOrEmpty(msisdn))
                {
                    return new ServiceResult(false, "MSISDN  is Required");
                }

                if (string.IsNullOrEmpty(dateTimeReq))
                {
                    return new ServiceResult(false, "DateTimeReq  is Required");
                }

                if (string.IsNullOrEmpty(meterNumber))
                {
                    return new ServiceResult(false, "Meter Number  is Required");
                }

                if (meterNumber.Length != 12)
                {
                    return new ServiceResult(false, "The MeterNumber must be a 12-digit.");
                }


                if (amount < 3)
                {
                    return new ServiceResult(false, "The Amount must be at least 3.");
                }

                var newToken = new IssueTkn
                {
                    ConversationID = conversationId,
                    MSISDN = msisdn,
                    DateTimeReq = dateTimeReq,
                    Amount = amount,
                    UniqueNumber = uniqueNumber,
                    MeterNumber = meterNumber,

                };
                var result = await AddIssueToken(newToken);

                return new ServiceResult(result, result ? "The saving process was successful." : "Unknown error");

            }
            catch (Exception e)
            {
                return new ServiceResult(false, e.Message);
            }
        }

        public async Task<List<IssueTkn>> GetAll()
        {
            return await _context.IssueTkns.ToListAsync();
        }



      
    }
}
