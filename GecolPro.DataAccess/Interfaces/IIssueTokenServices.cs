using GecolPro.Models.DbEntity;

namespace GecolPro.DataAccess.Interfaces;

public interface IIssueTokenServices
{
    Task<ServiceResult> CreateNew(string? conversationId, string? msisdn, string? dateTimeReq, string? uniqueNumber, string? meterNumber, int amount);


    Task<List<IssueTkn>> GetAll();

  

}