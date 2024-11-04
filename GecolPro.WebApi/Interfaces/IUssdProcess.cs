using GecolPro.Models.Models;
using static GecolPro.Models.Models.MultiRequestUSSD;

namespace GecolPro.WebApi.Interfaces
{
    public interface IUssdProcess
    {
        public Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang);
        public Task<bool> CheckServiceExist(string? convID = null);
        public Task<bool> CheckDcbExist(string? convID = null);

    }
}
