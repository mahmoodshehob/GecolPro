using GecolPro.DataAccess.Interfaces;
using GecolPro.Models.DbEntity;
using Microsoft.EntityFrameworkCore;


namespace GecolPro.DataAccess.Services
{
    public class DatabaseConnection : IDatabaseConnection
    {





        private readonly AppDbContext _context;

        public DatabaseConnection(AppDbContext context)
        {
            _context = context;
        }



        public async Task<ServiceResult> CheckConnection()
        {
            try
            {
                if(_context.Database.CanConnect())
                    return new ServiceResult(true, "Database Connected Successfully");

                return new ServiceResult(false, "Can't Connect with Database");
 
            }
            catch (Exception ex)
            {              
                return new ServiceResult(false, ex.Message);
            }
        }
    }
}
