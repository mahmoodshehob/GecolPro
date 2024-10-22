using GecolPro.Models.Models;
using static GecolPro.Models.Models.MultiRequestUSSD;

namespace GecolPro.WebApi.Interfaces
{
    public interface IUssdProcessV1
    {
        public Task<MultiResponseUSSD> ServiceProcessing(MultiRequest multiRequest, string Lang);
        public Task<bool> CheckServiceExist();
        public Task<bool> CheckDcbExist();

    }
}
