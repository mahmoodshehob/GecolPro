using GecolPro.Models.Models;
using static GecolPro.Models.Models.MultiRequestUSSD;

namespace GecolPro.WebApi.Interfaces
{
    public interface IUssdProcess
    {
        public Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang);
        public Task<bool> CheckServiceExist();
        public Task<bool> CheckDcbExist();

    }
}
