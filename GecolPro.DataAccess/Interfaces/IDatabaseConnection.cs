using GecolPro.Models.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.DataAccess.Interfaces
{
    public interface IDatabaseConnection
    {
        public Task<ServiceResult> CheckConnection();
    }
}
