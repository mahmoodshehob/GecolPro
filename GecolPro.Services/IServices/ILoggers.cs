using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Services.IServices
{
    public interface ILoggers
    {
        public Task LogInfoAsync(string message);

        public Task LogConnectionsStatusAsync(string message);
        
        public Task LogErrorAsync(string message);
        
        public Task LogDebugAsync(string message);
        
        public Task LogUssdTransAsync(string message);
        
        public Task LogDcbTransAsync(string message); 
        
        public Task LogGecolTransAsync(string message);

        public Task LogIssuedTokenAsync(string message);

        public Task LogRequstDbAsync(string message);

    }
}
