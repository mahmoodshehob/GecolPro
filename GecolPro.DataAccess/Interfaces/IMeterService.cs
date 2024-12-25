using GecolPro.Models.DbEntity;
using GecolPro.Models.Gecol;

namespace GecolPro.DataAccess.Interfaces
{
    public interface IMeterService
    {
        Task<ServiceResult> CreateNew(string? meterNumber, string at, string tt, CustVendDetail? custVendDetail = null);
        Task<ServiceResult> IsExist(string? meterNumber);

        Task<List<Meter>> GetAll();
    }
}
