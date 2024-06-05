using ClassLibrary.DataAccess.Models;

namespace ClassLibrary.DataAccess.Interfaces
{
    public interface IMeterService
    { 
        Task<ServiceResult> CreateNew(string? meterNumber,string at,string tt); 
        Task<ServiceResult> IsExist(string? meterNumber);

        Task<List<Meter>> GetAll();
    }
}
