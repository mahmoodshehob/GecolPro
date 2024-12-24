using GecolPro.SmppClient.Services.IServices;

namespace GecolPro.SmppClient.Services
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
