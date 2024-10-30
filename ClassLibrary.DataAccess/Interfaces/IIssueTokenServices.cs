using ClassLibrary.DataAccess.Models;

namespace ClassLibrary.DataAccess.Interfaces;

public interface IIssueTokenServices
{
    Task<ServiceResult> CreateNew(string? conversationId, string? msisdn, string? dateTimeReq,string? uniqueNumber,string? meterNumber,int amount);


    Task<List<IssueTkn>> GetAll();

}