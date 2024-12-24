using ZGecolPro.SmppClient.Services.IServices;

namespace ZGecolPro.SmppClient.Services
{
    public class GuidService : IGuidService
    {
        private readonly Guid Id;

        public GuidService()
        {
            Id = Guid.NewGuid();
        }

        public string GetGuid() 
        { 
            return Id.ToString("N");        
        }        
    }
}
