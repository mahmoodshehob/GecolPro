using GecolPro.DataAccess.Interfaces;
using GecolPro.Models.DbEntity;
using Microsoft.EntityFrameworkCore;

namespace GecolPro.DataAccess.Services
{
    public class MeterService : IMeterService
    {
        private readonly AppDbContext _context;

        public MeterService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> IsMeterExist(string meterNumber)
        {
            return _context.Meters != null && await _context.Meters.AnyAsync(m => m.Number == meterNumber && m.IsActive);
        }

        private async Task<bool> AddMeter(Meter meter)
        {
            _context.Meters?.Add(meter);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ServiceResult> CreateNew(string? meterNumber, string? at, string? tt)
        {
            try
            {
                if (meterNumber == null)
                {
                    return new ServiceResult(false, "Meter Number is Required");
                }

                if (await IsMeterExist(meterNumber))
                {
                    return new ServiceResult(false, "Meter Number Already exists");
                }

                var newMeter = new Meter
                {
                    at = at,
                    tt = tt,
                    Number = meterNumber,
                };
                var result = await AddMeter(newMeter);

                return new ServiceResult(result, result ? "The saving process was successful." : "Unknown error");

            }
            catch (Exception e)
            {
                return new ServiceResult(false, e.Message);
            }


        }

        public async Task<ServiceResult> IsExist(string? meterNumber)
        {
            if (meterNumber == null)
            {
                return new ServiceResult(false, "Meter Number is Required");
            }

            if (await IsMeterExist(meterNumber))
            {
                return new ServiceResult(true, "Meter Number is exists");
            }
            return new ServiceResult(false, "Meter Number not exists");

        }

        public async Task<List<Meter>> GetAll()
        {
            return await _context.Meters.ToListAsync();
        }
    }
}
