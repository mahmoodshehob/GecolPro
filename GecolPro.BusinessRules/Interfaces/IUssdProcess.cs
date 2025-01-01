using GecolPro.Models.Models;
using Microsoft.AspNetCore.Mvc;
using static GecolPro.Models.Models.MultiRequestUSSD;

namespace GecolPro.BusinessRules.Interfaces
{
    public interface IUssdProcess
    {
        public Task<ContentResult> GetResponse(string xmlContent, string lang);

        public Task<(bool, string)> GetQueryTokensResponseSupport(string Msisdn, string OrderdNumber, string lang);
        public Task<ContentResult> GetQueryTokensResponse(string xmlContent, string lang);


        public Task<(bool, string)> CheckMeterInGecolWithDet(string meterNumber);
        public Task<bool> CheckServiceExist(string? convID = null);
        public Task<bool> CheckDcbExist(string? convID = null);

    }
}
